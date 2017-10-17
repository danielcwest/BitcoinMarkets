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
using BMCore.Engine;

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
            var threshold = configuration.GetValue<decimal>("TradeThreshold");
            var dbService = new BMDbService(configuration.GetValue<string>("SqlConnectionString"));
            var exchanges = configs.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);

            try
            {

                string num = 1233214.234235465344566m.ToString("#.####");

                var hitbtc = (Hitbtc)exchanges["Hitbtc"];
                var bittrex = (Bittrex)exchanges["Bittrex"];

                //      var order = hitbtc.CheckOrder("4189718031").Result;
                //     var order2 = hitbtc.CheckOrder("4189700957").Result;
                //EngineHelper.UpdateWithdrawalStatus(dbService, exchanges).Wait();

                var symbols = hitbtc.Symbols().Result.ToDictionary(s => s.LocalSymbol);
                decimal arbBuy = 0.00008500m;

                var res = EngineHelper.Buy(hitbtc, symbols["SNTETH"], dbService, "SNTETH", threshold / arbBuy, arbBuy).Result;

                Console.WriteLine("Complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
