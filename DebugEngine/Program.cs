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
using BMCore.Config;
using BMCore;
using PoloniexSharp;
using LiquiSharp;
using Newtonsoft.Json;

namespace DebugEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.log.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            var arbitrageConfig = new ArbitrageConfig();
            configuration.GetSection("ArbitrageConfig").Bind(arbitrageConfig);

            var dbService = new BMDbService(configuration.GetValue<string>("SqlConnectionString"), arbitrageConfig.Gmail);
            var exchanges = arbitrageConfig.Exchanges.Where(c => c.Enabled).Select(c => ExchangeFactory.GetInstance(c)).ToDictionary(e => e.Name);
            var baseCurrencies = arbitrageConfig.BaseCurrencies.Where(c => c.Enabled).ToDictionary(e => e.Name);

            try
            {
                var bittrex = (Bittrex)exchanges["Bittrex"];
                var hitbtc = (Hitbtc)exchanges["Hitbtc"];

                EngineHelper.ProcessTransactions(dbService, exchanges).Wait();

                //var deposits = bittrex.GetDeposits().Result;

                //  var bOrder = bittrex.CheckOrder("181f9c8b-4a40-449e-9d06-b2db7530ec0a").Result;
                //  var withd = bittrex.GetWithdrawals().Result;


                //var hOrder = hitbtc.CheckOrder("4688283773").Result;
                //  var hWith = hitbtc.GetWithdrawal("20f7e751-a8fd-4acc-8931-3396dfc919ed").Result;

                Console.WriteLine("Complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
