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
        Dictionary<string, ICurrencyBalance> baseExchangeBalances;

        //ARBITRAGE EXCHANGE
        IExchange arbExchange;
        Dictionary<string, ISymbol> arbitrageExchangeMarkets;
        Dictionary<string, ICurrencyBalance> arbitrageExchangeBalances;

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

        public async Task RefreshBalances()
        {
            var baseBalances = await this.baseExchange.GetBalances();
            var arbBalances = await this.arbExchange.GetBalances();
            this.baseExchangeBalances = baseBalances.ToDictionary(b => b.Currency);
            this.arbitrageExchangeBalances = arbBalances.ToDictionary(b => b.Currency);
            await Task.FromResult(0);
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
                            await FindOpportunity(new ArbitrageMarket(bMarket, bBook, rMarket, rBook));
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

        public async Task FindOpportunity(ArbitrageMarket am)
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;

            var baseBase = this.baseExchangeBalances[am.baseMarket.BaseCurrency].Amount;
            var baseQuote = this.baseExchangeBalances[am.baseMarket.QuoteCurrency].Amount * am.baseMarket.Last;
            var arbBase = this.arbitrageExchangeBalances[am.arbitrageMarket.BaseCurrency].Amount;
            var arbQuote = this.arbitrageExchangeBalances[am.arbitrageMarket.QuoteCurrency].Amount * am.arbitrageMarket.Last;

            baseBuy = GetPriceAtVolumeThreshold(txThreshold, am.baseBook.asks);
            baseSell = GetPriceAtVolumeThreshold(txThreshold, am.baseBook.bids);
            arbBuy = GetPriceAtVolumeThreshold(txThreshold, am.arbitrageBook.asks);
            arbSell = GetPriceAtVolumeThreshold(txThreshold, am.arbitrageBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (this.baseExchange.Fee + this.arbExchange.Fee);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (this.baseExchange.Fee + this.arbExchange.Fee);

                //Execute trades
                if (baseBuy < arbSell && baseBuySpread >= 0.01m && baseBase > 2m * txThreshold && arbQuote > 2m * txThreshold)
                {
                    //basebuy arbsell
                    long baseId = await EngineHelper.Buy(this.baseExchange, this.baseExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold / baseBuy, baseBuy);
                    long arbId = await EngineHelper.Sell(this.arbExchange, this.arbitrageExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold, arbSell);
                    dbService.SaveOrderPair(baseId, arbId);

                }
                else
                {
                    dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, "FindOpportunity", "Insufficient Funds", "");
                }

                if (baseSell > arbBuy && baseSellSpread >= 0.01m && baseQuote > 2m * txThreshold && arbBase > 2m * txThreshold)
                {
                    //arbbuy basesell
                    long baseId = await EngineHelper.Sell(this.baseExchange, this.baseExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold, baseSell);
                    long arbId = await EngineHelper.Buy(this.arbExchange, this.arbitrageExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold / arbBuy, arbBuy);
                    dbService.SaveOrderPair(baseId, arbId);
                }
                else
                {
                    dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, "FindOpportunity", "Insufficient Funds", "");
                }

                if (baseBuy < arbSell && baseBuySpread > 0)
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, baseBuy, arbSell, baseBuySpread);
                }
                if (baseSell > arbBuy && baseSellSpread > 0)
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, baseSell, arbBuy, baseSellSpread);
                }
            }
            await Task.FromResult(0);
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