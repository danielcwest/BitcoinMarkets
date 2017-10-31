using BittrexSharp;
using BMCore.Config;
using BMCore.DbService;
using BMCore.Engine;
using Engine;
using HitbtcSharp;
using Microsoft.Extensions.Configuration;
using RestEase;
using System;
using System.IO;
using System.Linq;
using System.Threading;

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

                var bittrex = (Bittrex)exchanges["Bittrex"];
                var hitbtc = (Hitbtc)exchanges["Hitbtc"];

                var maker = new MarketMaker(hitbtc, bittrex, dbService);

                while (true)
                {
                    maker.ResetLimitOrders(-1).Wait();
                    Console.WriteLine("Complete Sleeping...");
                    Thread.Sleep(1000 * 60);
                }

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
