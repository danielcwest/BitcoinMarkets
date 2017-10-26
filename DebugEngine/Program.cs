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
                var bittrex = exchanges["Bittrex"];
                var hitbtc = exchanges["Hitbtc"];

                var bOrder = bittrex.CheckOrder("759382c3-579a-4384-b83c-9d86c5a10180").Result;
                var hOrder = hitbtc.CheckOrder("4616377485").Result;

                var bWithdrawal = bittrex.GetWithdrawal("cc5b08ce-d735-4afa-b4d6-a7fcd3abc863").Result;
                var hWithdrawal = hitbtc.GetWithdrawal("b7ba911e-4f7c-4b1c-be4f-ce4503b06659").Result;

                Console.WriteLine("Complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
