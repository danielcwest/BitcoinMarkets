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

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    PrintUsage();
                    return;
                }

                Console.WriteLine("Starting Engine ...");

                string configFile = args[0] == "log" ? "appsettings.log.json" : "appsettings.json";

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                IConfigurationRoot configuration = builder.Build();
                var arbitrageConfig = new ArbitrageConfig();
                configuration.GetSection("ArbitrageConfig").Bind(arbitrageConfig);

                var dbService = new BMDbService(configuration.GetValue<string>("SqlConnectionString"), arbitrageConfig.Gmail);
                var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                switch (args[0])
                {
                    case "log":
                        
                        break;
                    case "make":
                        var maker = new MarketMaker((Hitbtc)exchanges["Hitbtc"], (Bittrex)exchanges["Bittrex"], dbService);
                        while (true)
                        {
                            int pId = dbService.StartEngineProcess("Hitbtc", "Bittrex", "marketmaking", new CurrencyConfig());
                            maker.ResetLimitOrders(-1).Wait();
                            maker.ProcessOrders(-1).Wait();
                            Console.WriteLine("Complete Sleeping...");
                            Thread.Sleep(1000 * 60);
                        }
                        break;
                    case "balance":

                        break;
                    case "report":

                        break;
                    default:
                        break;
                }
                Console.WriteLine("Engine Complete...");
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
