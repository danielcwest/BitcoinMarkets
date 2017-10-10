using System;
using System.Globalization;
using System.IO;
using System.Threading;
using BinanceSharp;
using BittrexSharp;
using HitbtcSharp;
using Microsoft.Extensions.Configuration;
using PoloniexSharp;

namespace Engine
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

            Console.WriteLine("Starting Engine ...");

            while (true)
            {
                try
                {
                    var bittrex = new Bittrex(configuration.GetValue<string>("Exchanges:Bittrex:ApiKey"), configuration.GetValue<string>("Exchanges:Bittrex:Secret"));
                    var hitbtc = new Hitbtc();
                    var binance = new Binance();

                    Console.WriteLine("Bittrex Hitbtc");

                    var engine = new TradingEngine(bittrex, hitbtc);
                    engine.LoadMarketData().Wait();
                    engine.FindOpportunities();
                    engine.PrintResults();

                    Console.WriteLine("Bittrex Binance");

                    engine = new TradingEngine(bittrex, binance);
                    engine.LoadMarketData().Wait();
                    engine.FindOpportunities();
                    engine.PrintResults();

                    Console.WriteLine("Hitbtc Binance");

                    engine = new TradingEngine(hitbtc, binance);
                    engine.LoadMarketData().Wait();
                    engine.FindOpportunities();
                    engine.PrintResults();

                    Console.WriteLine("Complete");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    Console.WriteLine("Sleeping ...");
                    Thread.Sleep(1000);
                }
            }

        }
    }
}
