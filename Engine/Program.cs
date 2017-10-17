using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using BMCore.Contracts;

using BinanceSharp;
using BittrexSharp;
using BitzSharp;
using HitbtcSharp;
using LiquiSharp;
using LivecoinSharp;
using NovaExchangeSharp;
using PoloniexSharp;
using TidexSharp;
using System.Linq;
using BMCore.Models;
using BMCore.DbService;
using BMCore.Engine;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting Engine ...");

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                IConfigurationRoot configuration = builder.Build();
                var configs = new List<ConfigExchange>();
                configuration.GetSection("Exchanges").Bind(configs);
                var threshold = configuration.GetValue<decimal>("TradeThreshold");
                var dbService = new BMDbService(configuration.GetValue<string>("SqlConnectionString"));
                var exchanges = configs.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                if (args.Length < 1)
                {
                    PrintUsage();
                    return;
                }

                switch (args[0])
                {
                    case "log":
                        if (args.Length == 1)
                        {
                            EngineHelper.ExecuteAllExchanges(exchanges.Values.ToArray(), dbService, threshold, "log");
                        }
                        else if (args.Length == 3)
                        {
                            EngineHelper.ExecuteExchangePair(exchanges[args[1]], exchanges[args[2]], dbService, threshold, "log");
                        }
                        break;
                    case "trade":
                        if (args.Length == 3)
                        {
                            if (dbService.GetInvalidOrderCount() > 0)
                                throw new Exception("Fix Invalid orders");

                            EngineHelper.ExecuteExchangePair(exchanges[args[1]], exchanges[args[2]], dbService, threshold, "trade");

                        }
                        break;
                    case "withdraw":
                        EngineHelper.UpdateOrderStatus(dbService, exchanges).Wait();

                        EngineHelper.ProcessWithdrawals(dbService, exchanges, threshold).Wait();

                        Thread.Sleep(1000 * 60 * 3); //Wait 3 minutes for withdrawals to go through on exchange

                        EngineHelper.UpdateWithdrawalStatus(dbService, exchanges).Wait();
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
