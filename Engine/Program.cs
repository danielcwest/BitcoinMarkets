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

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Config ...");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var config = new List<ConfigExchange>();
            configuration.GetSection("Exchanges").Bind(config);

            var sqlConnectionString = configuration.GetValue<string>("SqlConnectionString");
            var dbService = new DbService(sqlConnectionString);

            var exchanges = config.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToArray();

            Console.WriteLine("Starting Engine ...");

            while (true)
            {
                for (var i = 0; i < exchanges.Length; i++)
                {
                    var baseExchange = exchanges[i];
                    for (var j = 0; j < exchanges.Length; j++)
                    {
                        var arbExchange = exchanges[j];

                        if (baseExchange != arbExchange)
                        {
                            try
                            {
                                Console.WriteLine("Starting: {0} {1}", baseExchange.GetExchangeName(), arbExchange.GetExchangeName());
                                var engine = new TradingEngine(baseExchange, arbExchange, dbService);
                                engine.AnalyzeMarkets().Wait();
                                Console.WriteLine("Completed: {0} {1}", baseExchange.GetExchangeName(), arbExchange.GetExchangeName());

                            }
                            catch (Exception e)
                            {
                                //Console.WriteLine(e);
                                Console.WriteLine("Error: {0} {1}", baseExchange.GetExchangeName(), arbExchange.GetExchangeName());
                                dbService.LogError(baseExchange.GetExchangeName(), arbExchange.GetExchangeName(), "", "Main", e.Message, e.StackTrace);

                            }
                            finally
                            {
                                //Thread.Sleep(1000 * 60);
                                Thread.Sleep(100);
                            }
                        }

                    }
                }
            }
        }
    }
}
