using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BinanceSharp;
using BittrexSharp;
using BMCore.Contracts;
using BMCore.DbService;
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

        DbService dbService;

        public TradingEngine(IExchange baseExchange, IExchange arbExchange, DbService dbService)
        {
            this.dbService = dbService;
            this.baseExchange = baseExchange;
            this.arbExchange = arbExchange;
            this.arbitrageMarkets = new ConcurrentDictionary<string, ArbitrageMarket>();
            results = new ConcurrentBag<string>();
        }

        public async Task AnalyzeMarkets()
        {
            var baseMarkets = await this.baseExchange.MarketSummaries();
            this.baseExchangeMarkets = baseMarkets.ToDictionary(m => m.MarketName);

            var arbExchange = await this.arbExchange.MarketSummaries();
            this.arbitrageExchangeMarkets = arbExchange.Where(m => !string.IsNullOrWhiteSpace(m.MarketName)).ToDictionary(m => m.MarketName);

            foreach (var kvp in this.baseExchangeMarkets)
            {
                if (this.arbitrageExchangeMarkets.ContainsKey(kvp.Key))
                {
                    try
                    {
                        //Always get the freshest data
                        var bMarket = await this.baseExchange.MarketSummary(kvp.Key);
                        var bBook = await this.baseExchange.OrderBook(kvp.Key);
                        var rMarket = await this.arbExchange.MarketSummary(kvp.Key);
                        var rBook = await this.arbExchange.OrderBook(kvp.Key);

                        if (bMarket == null || bBook == null || rMarket == null || rBook == null)
                        {
                            dbService.LogError(this.baseExchange.GetExchangeName(), this.arbExchange.GetExchangeName(), kvp.Key, "AnalyzeMarkets", "Market Data Null", "");
                        }
                        else
                        {
                            FindOpportunity(new ArbitrageMarket(kvp.Value, bBook, this.arbitrageExchangeMarkets[kvp.Key], rBook));
                        }
                    }
                    catch (Exception e)
                    {
                        dbService.LogError(this.baseExchange.GetExchangeName(), this.arbExchange.GetExchangeName(), kvp.Key, "AnalyzeMarkets", e.Message, e.StackTrace);
                    }
                }
            }

            await Task.FromResult(0);
        }

        public void FindOpportunity(ArbitrageMarket am)
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;

            baseBuy = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.baseBook.asks);
            baseSell = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.baseBook.bids);
            arbBuy = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.arbitrageBook.asks);
            arbSell = GetPriceAtVolumeThreshold(TX_THRESHOLD, am.arbitrageBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (this.baseExchange.GetFee() + this.arbExchange.GetFee());
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (this.baseExchange.GetFee() + this.arbExchange.GetFee());

                if (baseBuy < arbSell && baseBuySpread > 0)
                {
                    dbService.LogTrade(this.baseExchange.GetExchangeName(), this.arbExchange.GetExchangeName(), am.Symbol, baseBuy, arbSell, baseBuySpread);
                }
                if (baseSell > arbBuy && baseSellSpread > 0)
                {
                    dbService.LogTrade(this.baseExchange.GetExchangeName(), this.arbExchange.GetExchangeName(), am.Symbol, baseSell, arbBuy, baseSellSpread);
                }
            }
        }

        private decimal GetPriceAtVolumeThreshold(decimal threshold, List<OrderBookEntry> entries)
        {
            decimal result = -1M;
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
    }
}