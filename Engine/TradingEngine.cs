using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BinanceSharp;
using BittrexSharp;
using BMCore.Contracts;
using BMCore.Models;
using HitbtcSharp;
using Microsoft.Extensions.Configuration;
using PoloniexSharp;

namespace Engine
{
    public class TradingEngine
    {
        public const decimal TX_THRESHOLD = 0.25M;

        // BASE EXCHANGE
        IExchange baseExchange;
        Dictionary<string, IMarket> baseExchangeMarkets;

        //ARBITRAGE EXCHANGE
        IExchange arbExchange;
        Dictionary<string, IMarket> arbitrageExchangeMarkets;

        ConcurrentDictionary<string, ArbitrageMarket> arbitrageMarkets;

        ConcurrentBag<string> results;

        public TradingEngine(IExchange baseExchange, IExchange arbExchange)
        {
            this.baseExchange = baseExchange;
            this.arbExchange = arbExchange;
            this.arbitrageMarkets = new ConcurrentDictionary<string, ArbitrageMarket>();
            results = new ConcurrentBag<string>();
        }

        public async Task LoadMarketData()
        {
            var baseMarkets = await this.baseExchange.MarketSummaries();
            this.baseExchangeMarkets = baseMarkets.ToDictionary(m => m.MarketName);

            var arbExchange = await this.arbExchange.MarketSummaries();
            this.arbitrageExchangeMarkets = arbExchange.ToDictionary(m => m.MarketName);

            foreach (var kvp in this.baseExchangeMarkets)
            {
                if (this.arbitrageExchangeMarkets.ContainsKey(kvp.Key))
                {
                    try
                    {
                        var bBook = await this.baseExchange.OrderBook(kvp.Key);
                        var rBook = await this.arbExchange.OrderBook(kvp.Key);
                        this.arbitrageMarkets.TryAdd(kvp.Key, new ArbitrageMarket(kvp.Value, bBook, this.arbitrageExchangeMarkets[kvp.Key], rBook));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(kvp.Key);
                        Console.WriteLine(e);
                    }

                }
            }

            await Task.FromResult(0);
        }

        public void FindOpportunities()
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;
            foreach (var am in this.arbitrageMarkets.Values)
            {
                baseBuy = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.baseBook.asks);
                baseSell = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.baseBook.bids);
                arbBuy = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.arbitrageBook.asks);
                arbSell = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.arbitrageBook.bids);

                if (baseBuy != 0M && baseSell != 0M && arbBuy != 0M && arbSell != 0M)
                {
                    baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (this.baseExchange.GetFee() + this.arbExchange.GetFee());
                    baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (this.baseExchange.GetFee() + this.arbExchange.GetFee());

                    if (baseBuy < arbSell && baseBuySpread > 0)
                    {
                        results.Add(string.Format("{7}-{8} {0}: {1} Buy @{2:N8}, {3} Sell @{4:N8}, Spread: {5:P2}, {6:F}", am.Symbol, am.baseMarket.Exchange, baseBuy, am.arbitrageMarket.Exchange, arbSell, baseBuySpread, DateTime.Now, this.baseExchange.GetExchangeName(), this.arbExchange.GetExchangeName()));
                    }
                    if (baseSell > arbBuy && baseSellSpread > 0)
                    {
                        results.Add(string.Format("{7}-{8} {0}: {1} Sell @{2:N8}, {3} Buy @{4:N8}, Spread: {5:P2}, {6:F}", am.Symbol, am.baseMarket.Exchange, baseSell, am.arbitrageMarket.Exchange, arbBuy, baseSellSpread, DateTime.Now, this.baseExchange.GetExchangeName(), this.arbExchange.GetExchangeName()));
                    }
                }
            }
        }

        private decimal GetPriceAtVolumeThreshold(decimal threshold, List<OrderBookEntry> entries)
        {
            decimal result = 0M;
            foreach (var entry in entries)
            {
                if (entry.sumBase >= threshold)
                {
                    result = entry.price;
                    break;
                }
            }
            return result;
        }

        public void PrintResults()
        {
            using (StreamWriter sw = File.AppendText("out.txt"))
            {
                foreach (string line in results)
                {
                    sw.WriteLine(line);
                }
            }
        }

    }
}