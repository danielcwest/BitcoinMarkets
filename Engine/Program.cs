using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using BMCore.Contracts;
using BinanceSharp;
using BittrexSharp;
using HitbtcSharp;
using System.Linq;
using BMCore.Models;
using BMCore.DbService;
using BMCore.Engine;
using BMCore.Config;
using RestEase;
using GdaxSharp;
using System.Threading.Tasks;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //dotnet publish -c Release -r win10-x64

                Console.WriteLine("Starting Engine ...");

                string configFile = "appsettings.json";

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                IConfigurationRoot configuration = builder.Build();
                var arbitrageConfig = new ArbitrageConfig();
                configuration.GetSection("ArbitrageConfig").Bind(arbitrageConfig);

                var dbService = new BMDbService(configuration.GetValue<string>("SqlConnectionString"), arbitrageConfig.Gmail);
                var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                var hitbtc = (Hitbtc)exchanges["Hitbtc"];
                var gdax = (Gdax)exchanges["Gdax"];

                var marketMaker = new MarketMaker(gdax, hitbtc, dbService, arbitrageConfig.Gmail);
                var reporter = new EngineReporter(gdax, hitbtc, dbService);

                int timeout = 3;
                if (args.Length > 0)
                    timeout = int.Parse(args[0]);

                while (true)
                {
                    EngineHelper.ExecuteMarketPairs(dbService, exchanges, arbitrageConfig.Gmail).Wait();
                    Console.WriteLine("Complete, sleeping");
                    Thread.Sleep(1000 * timeout);
                }

               // Console.WriteLine("Engine Complete...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
