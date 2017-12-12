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

                //hitbtcv2.InitWebSocket();
                //hitbtcv2.Login().Wait();

                //hitbtcv2.ImmediateOrCancel("sell", "ETHBTC", 1m, 0.1m).Wait();

                bManager.StartManager();

                 // enginev2.StartEngine(hitbtcv2);

                // EngineHelper.TakeProfit(dbService, hitbtc, binance, 0.08m);
                //enginev2.StartEngine(hitbtcv2);
                //       var addr = hitbtc.GetDepositAddress("ETH").Result;
                //     var res = binance.Withdraw(addr.Currency, 3m, addr.Address).Result;

                //  var addr = binance.GetDepositAddress("ETC").Result;
                //  var res = hitbtc.Withdraw(addr.Currency, 22.12471398m, addr.Address).Result;

                //var res = hitbtc.Withdraw("DOGE", 1000000m, "DDi3FWrU7RhSJ4NbYKS9gAM73ZHP8TCQTv").Result;
                //AuditOrder("XMRBTC", "8903177267", "1614791", hitbtc, binance);

                Console.ReadKey();

                //var result = gdax.MarketSell("BTCUSD", 0.63405632m).Result;

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