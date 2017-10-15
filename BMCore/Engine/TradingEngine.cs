using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;

namespace BMCore.Engine
{
    public class TradingEngine
    {
        decimal txThreshold;

        // BASE EXCHANGE
        IExchange baseExchange;
        Dictionary<string, ISymbol> baseExchangeMarkets;

        //ARBITRAGE EXCHANGE
        IExchange arbExchange;
        Dictionary<string, ISymbol> arbitrageExchangeMarkets;

        ConcurrentDictionary<string, ArbitrageMarket> arbitrageMarkets;

        ConcurrentBag<string> results;

        BMDbService dbService;

        public TradingEngine(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, decimal txThreshold)
        {
            this.dbService = dbService;
            this.baseExchange = baseExchange;
            this.arbExchange = arbExchange;
            this.arbitrageMarkets = new ConcurrentDictionary<string, ArbitrageMarket>();
            results = new ConcurrentBag<string>();
            this.txThreshold = txThreshold;
        }

        public async Task AnalyzeMarkets()
        {
            var baseMarkets = await this.baseExchange.Symbols();
            this.baseExchangeMarkets = baseMarkets.ToDictionary(m => m.LocalSymbol);

            var arbExchange = await this.arbExchange.Symbols();
            this.arbitrageExchangeMarkets = arbExchange.ToDictionary(m => m.LocalSymbol);

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
                            dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, kvp.Key, "AnalyzeMarkets", "Market Data Null", "");
                            Console.WriteLine("{0}: Null Market Data", kvp.Key);
                        }
                        else
                        {
                            FindOpportunity(new ArbitrageMarket(bMarket, bBook, rMarket, rBook));
                            Console.WriteLine("Analyzing {0}", kvp.Key);
                        }
                    }
                    catch (Exception e)
                    {
                        dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, kvp.Key, "AnalyzeMarkets", e.Message, e.StackTrace);
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

            baseBuy = GetPriceAtVolumeThreshold(txThreshold, am.baseBook.asks);
            baseSell = GetPriceAtVolumeThreshold(txThreshold, am.baseBook.bids);
            arbBuy = GetPriceAtVolumeThreshold(txThreshold, am.arbitrageBook.asks);
            arbSell = GetPriceAtVolumeThreshold(txThreshold, am.arbitrageBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (this.baseExchange.Fee + this.arbExchange.Fee);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (this.baseExchange.Fee + this.arbExchange.Fee);

                if (baseBuy < arbSell && baseBuySpread > 0)
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, baseBuy, arbSell, baseBuySpread);
                }
                if (baseSell > arbBuy && baseSellSpread > 0)
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, baseSell, arbBuy, baseSellSpread);
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