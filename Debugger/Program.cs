using BinanceSharp;
using BittrexSharp;
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
using WebSocketSharp;

namespace Debugger
{
    class Program
    {

        //dotnet publish -c Release -r win10-x64

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

                var configs = arbitrageConfig.Exchanges.ToDictionary(c => c.Name);
                var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

                var binance = new Binance(configs["Binance"]);//(Binance)exchanges["Binance"];
                var hitbtc = (Hitbtc)exchanges["Hitbtc"];
                var gdax = (Gdax)exchanges["Gdax"];


                var hitbtcv2 = new HitbtcSocket(configs["Hitbtc"]);

                var enginev2 = new ArbitrageEngineV2(hitbtcv2.Rest, binance, dbService);
                var bManager = new BalanceManager(dbService, hitbtcv2.Rest, binance);

            //    bManager.StartManager();

                var hitbtcBalances = hitbtc.GetBalances().Result.ToDictionary(b => b.Currency);
                var binanceBalances = binance.GetBalances().Result.ToDictionary(b => b.Currency);

                string address = "0xf18414d9961c458a0d120ffe2f0b0da279d1aea0";
                //string tag = "24da0a5343fc7a28199edd703b8c80bc7b84ab154b1ce4416e53b8f00437f01d";
                string currency = "ETH";

                    decimal hitAmount = hitbtcBalances[currency].Available;
                //decimal biAmount = binanceBalances[currency].Available;

                if (!string.IsNullOrWhiteSpace(address) && hitAmount > 0)
                {
                    logger.Trace("Withdraw {0} {1} to {2}", hitAmount, currency, address);
                    var hitRes = hitbtc.Withdraw(currency, hitAmount, address).Result;
                }

                //if (!string.IsNullOrWhiteSpace(address) && biAmount > 0)
                //{
                //    logger.Trace("Withdraw {0} {1} to {2}", biAmount, currency, address);
                //    var biRes = binance.Withdraw(currency, biAmount, address).Result;
                //}


                //var res = hitbtc.Withdraw("DOGE", 1000000m, "DDi3FWrU7RhSJ4NbYKS9gAM73ZHP8TCQTv").Result;
                //AuditOrder("XMRBTC", "8903177267", "1614791", hitbtc, binance);

                //       Console.ReadKey();

                //var result = gdax.MarketSell("BTCUSD", 0.63405632m).Result;

                //      logger.Trace("Complete");
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

        public static void AuditOrder(string symbol, string baseUuid, string counterUuid, IExchange baseExchange, IExchange counterExchange)
        {

            var baseOrder = baseExchange.CheckOrder(baseUuid, symbol).Result;
            var counterOrder = counterExchange.CheckOrder(counterUuid, symbol).Result;

            if (baseOrder != null && counterOrder != null)
            {
                decimal commission = 0m;
                if (baseOrder.Side == "buy")
                {
                    commission = counterOrder.CostProceeds - baseOrder.CostProceeds;
                }
                else
                {
                    commission = baseOrder.CostProceeds - counterOrder.CostProceeds;
                }
            }
        }
    }
}