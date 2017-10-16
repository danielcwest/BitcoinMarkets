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

                if (args.Length == 0)
                {
                    EngineHelper.ExecuteAllExchanges(exchanges.Values.ToArray(), dbService, threshold);
                }
                else if (args.Length == 2)
                {
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Finding Opportunities...");
                            EngineHelper.ExecuteExchangePair(exchanges[args[0]], exchanges[args[1]], dbService, threshold);

                            Console.WriteLine("Updating Order Statuses...");
                            EngineHelper.UpdateOrderStatus(dbService, exchanges).Wait();

                            Console.WriteLine("Processing Withdrawals");
                            EngineHelper.ProcessWithdrawals(dbService, exchanges).Wait();

                            Console.WriteLine("Updating Withdrawal Statuses...");
                            EngineHelper.UpdateWithdrawalStatus(dbService, exchanges).Wait();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            Console.WriteLine("Sleeping ...");
                            Thread.Sleep(1000 * 60);
                        }

                    }

                }

                Console.WriteLine("Engine Complete...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
