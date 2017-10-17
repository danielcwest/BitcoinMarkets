﻿using System;
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
                var hitbtc = (Hitbtc)exchanges["Hitbtc"];
                var bittrex = (Bittrex)exchanges["Bittrex"];

                decimal num = 123.124134556423453m;
                string n = num.ToString("N3");

                decimal txThreshold = .1m;
                decimal arbBuy = 0.002591m;
                var symbols = hitbtc.Symbols().Result.ToDictionary(s => s.ExchangeSymbol);
                long buyId = EngineHelper.Sell(hitbtc, symbols["ADXETH"], dbService, "ADXETH", txThreshold / arbBuy, arbBuy).Result;


                Console.WriteLine("Complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
