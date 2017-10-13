using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using BMCore.Models;
using HitbtcSharp;
using Microsoft.Extensions.Configuration;
using Engine;
using BMCore.DbService;
using HitbtcSharp.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BittrexSharp;

namespace DebugEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();
            var configs = new List<ConfigExchange>();
            configuration.GetSection("Exchanges").Bind(configs);

            var dbService = new DbService(configuration.GetValue<string>("SqlConnectionString"));
            var exchanges = configs.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

            try
            {
                var bittrex = (Bittrex)exchanges["Bittrex"];
                var hitbtc = (Hitbtc)exchanges["Hitbtc"];

                var engine = new TradingEngine(bittrex, hitbtc, dbService);
                engine.AnalyzeMarkets().Wait();


                Console.WriteLine("Complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
