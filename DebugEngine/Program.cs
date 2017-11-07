using BittrexSharp;
using BMCore.Config;
using BMCore.DbService;
using BMCore.Engine;
using BMCore.Models;
using Engine;
using GdaxSharp;
using HitbtcSharp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            var arbitrageConfig = new ArbitrageConfig();
            configuration.GetSection("ArbitrageConfig").Bind(arbitrageConfig);


            try
            {
                var dbService = new BMDbService(configuration.GetValue<string>("SqlConnectionString"), arbitrageConfig.Gmail);
                var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                var hitbtc = (Hitbtc)exchanges["Hitbtc"];
                var gdax = (Gdax)exchanges["Gdax"];

                var marketMaker = new MarketMaker(hitbtc, gdax, dbService, arbitrageConfig.Gmail);

                var result = hitbtc.LimitSell("123123123123", "ETHBTC", 1, 0.1m).Result;

                Console.WriteLine("Complete");
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.GetType() == typeof(ApiException))
                {
                    var apiE = (ApiException)e.InnerException;
                    Console.WriteLine(apiE.Content);
                }
                Console.WriteLine(e);
            }
        }
    }
}
