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
                var baseCurrencies = arbitrageConfig.BaseCurrencies.Where(c => c.Enabled).ToDictionary(e => e.Name);

                int timeout;
                int loopCount;

                if (!int.TryParse(args[1], out timeout))
                    timeout = 30;

                if (!int.TryParse(args[2], out loopCount))
                    loopCount = 3;

                int runCount = 0;

                switch (args[0])
                {
                    case "log":
                        while (runCount <= loopCount)
                        {
                            try
                            {
                                EngineHelper.LogOpportunities(dbService, exchanges).Wait();
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(ApiException))
                                {
                                    var apiE = (ApiException)ex.InnerException;
                                    dbService.LogError("Main", "Main", "Main", "log", apiE, -1);
                                }
                                Console.WriteLine(ex);
                            }
                            finally
                            {
                                runCount++;
                                Console.WriteLine("Complete, Sleeping {0} ...", runCount);
                                Thread.Sleep(1000 * timeout);
                            }
                        }
                        break;
                    case "trade":
                        while (runCount <= loopCount)
                        {
                            try
                            {
                                EngineHelper.ExecuteTradePairs(dbService, exchanges).Wait();
                                EngineHelper.ProcessTransactions(dbService, exchanges).Wait();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(ApiException))
                                {
                                    var apiE = (ApiException)ex.InnerException;
                                    dbService.LogError("Main", "Main", "Main", "trade", apiE, -1);
                                }
                            }
                            finally
                            {
                                runCount++;
                                Console.WriteLine("Complete, Sleeping {0} ...", runCount);
                                Thread.Sleep(1000 * timeout);
                            }
                        }
                        break;
                    case "balance":
                        try
                        {
                            EngineHelper.CheckExchangeBalances(dbService, exchanges).Wait();
                            Console.WriteLine("Complete, Sleeping ...");
                            Thread.Sleep(1000 * timeout);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            if (ex.InnerException != null && ex.InnerException.GetType() == typeof(ApiException))
                            {
                                var apiE = (ApiException)ex.InnerException;
                                dbService.LogError("Main", "Main", "Main", "balance", apiE, -1);
                            }
                        }
                        break;
                    case "report":
                        try
                        {
                            EngineReporter.GenerateEmailReport(dbService, exchanges, arbitrageConfig.Gmail).Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            dbService.LogError("Main", "Main", "Main", "report", ex, -1);
                        }
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
