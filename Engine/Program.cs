using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Core.Contracts;
using BinanceSharp;
using BittrexSharp;
using HitbtcSharp;
using System.Linq;
using Core.Models;
using Core.DbService;
using Core.Engine;
using Core.Config;
using RestEase;
using GdaxSharp;
using System.Threading.Tasks;
using NLog;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            NLog.Logger logger = LogManager.GetCurrentClassLogger();

            // Create the token source.
            var masterToken = new CancellationTokenSource();

            try
            {
                //dotnet publish -c Release -r win10-x64

                string configFile = "appsettings.json";

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                IConfigurationRoot configuration = builder.Build();
                var arbitrageConfig = new ArbitrageConfig();
                configuration.GetSection("ArbitrageConfig").Bind(arbitrageConfig);

                var dbService = new BMDbService(configuration.GetValue<string>("LocalSqlConnectionString"), arbitrageConfig.Gmail);

                var configs = arbitrageConfig.Exchanges.ToDictionary(c => c.Name);
                var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                var binance = (Binance)exchanges["Binance"];
                var hitbtcV2 = new HitbtcSocket(configs["Hitbtc"]);

                var balanceManager = new BalanceManager(dbService, hitbtcV2.Rest, binance);
                var engineV2 = new ArbitrageEngineV2(hitbtcV2.Rest, binance, dbService);

                balanceManager.StartManager();
                engineV2.StartEngine(hitbtcV2);              

            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("CommandLine Syntax: ");
            Console.WriteLine("log -- logs potential trades on all exhanges and all pairs");
            Console.WriteLine("log <BaseExchange> <ArbExchange> -- logs potential trades for given exchange pair");
            Console.ReadLine();
        }
    }
}
