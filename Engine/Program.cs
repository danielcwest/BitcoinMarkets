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

                var dbService = new DbService(configuration.GetValue<string>("SqlConnectionString"));
                var exchanges = configs.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                if (args.Length == 0)
                {
                    EngineHelper.ExecuteAllExchanges(exchanges.Values.ToArray(), dbService);
                }
                else if (args.Length == 2)
                {
                    EngineHelper.ExecuteExchangePair(exchanges[args[0]], exchanges[args[1]], dbService);
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
