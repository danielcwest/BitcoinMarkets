using BinanceSharp;
using Core;
using Core.Config;
using Core.Contracts;
using Core.DbService;
using Core.Engine;
using Core.Logging;
using Core.Models;
using Engine;
using GdaxSharp;
using HitbtcSharp;
using HitbtcSharp.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using OkexSharp;
using RestEase;
using System;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            #region config
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            var arbitrageConfig = new ArbitrageConfig();
            configuration.GetSection("ArbitrageConfig").Bind(arbitrageConfig);

            NLog.Logger logger = LogManager.GetCurrentClassLogger();

            // Create the token source.
            var masterToken = new CancellationTokenSource();

            #endregion
            try
            {
                var dbService = new BMDbService(configuration.GetValue<string>("LocalSqlConnectionString"), arbitrageConfig.Gmail);

                var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                var hitbtc = (Hitbtc)exchanges["Hitbtc"];
                var binance = (Binance)exchanges["Binance"];

                var engine = new ArbitrageEngine(hitbtc, binance, dbService, arbitrageConfig.Gmail);

                engine.StartEngine(masterToken.Token, 35);

                //    var trades = binance.GetTrades("ETHBTC").Result;
                // var gdax = (GdaxSocket)exchanges["Gdax"];
                //  var okex = (OkexSocket)exchanges["Okex"];
                //engine.StartEngine(masterToken.Token, 60);

                // hitbtc.SubscribeTrades("ETHBTC");

              //  Console.ReadKey();

                logger.Trace("Complete");
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.GetType() == typeof(ApiException))
                {
                    var apiE = (ApiException)e.InnerException;
                    Console.WriteLine(apiE.Content);
                }
                masterToken.Cancel();
                Console.WriteLine(e);
            }
        }
    }
}