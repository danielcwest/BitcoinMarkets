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

        string runType = "log";
        int pId = 0;

        public TradingEngine(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, decimal txThreshold)
        {
            this.dbService = dbService;
            this.baseExchange = baseExchange;
            this.arbExchange = arbExchange;
            this.arbitrageMarkets = new ConcurrentDictionary<string, ArbitrageMarket>();
            results = new ConcurrentBag<string>();
            this.txThreshold = txThreshold;
        }

        public async Task AnalyzeFundedPairs(int pId)
        {
            this.runType = "trade";
            this.pId = pId;
            var baseBalances = await this.baseExchange.GetBalances();
            var arbBalances = await this.arbExchange.GetBalances();
            this.baseExchangeBalances = baseBalances.Where(b => b.Available > 0).ToDictionary(b => b.Currency);
            this.arbitrageExchangeBalances = arbBalances.Where(b => b.Available > 0).ToDictionary(b => b.Currency);

            await RefreshSymbols();

            //Ethereum Markets
            if (BaseCurrencyAvailable("ETH"))
            {
                await AnalyzeEthereumPairs();
            }


            await Task.FromResult(0);
        }

        private bool BaseCurrencyAvailable(string currency)
        {
            return this.baseExchangeBalances.ContainsKey(currency) &&
            this.baseExchangeBalances[currency].Available > txThreshold
            && this.arbitrageExchangeBalances.ContainsKey(currency) &&
            this.arbitrageExchangeBalances[currency].Available > txThreshold;
        }




        private async Task AnalyzeEthereumPairs()
        {
            foreach (var kvp in this.baseExchangeBalances.Where(kvp => !EngineHelper.IsBaseCurrency(kvp.Value.Currency)))
            {
                if (this.arbitrageExchangeBalances.ContainsKey(kvp.Key))
                {
                    string symbol = string.Format("{0}ETH", kvp.Key);
                    await AnalyzeMarket(symbol);
                }
            }
            await Task.FromResult(0);
        }

        public async Task AnalyzeMarkets()
        {
            await RefreshSymbols();
            foreach (var kvp in this.baseExchangeMarkets)
            {
                if (this.arbitrageExchangeMarkets.ContainsKey(kvp.Key))
                {
                    await AnalyzeMarket(kvp.Key);
                }
            }

            await Task.FromResult(0);
        }

        public async Task RefreshSymbols()
        {
            var baseMarkets = await this.baseExchange.Symbols();
            this.baseExchangeMarkets = baseMarkets.ToDictionary(m => m.LocalSymbol);

            var arbExchange = await this.arbExchange.Symbols();
            this.arbitrageExchangeMarkets = arbExchange.ToDictionary(m => m.LocalSymbol);

            await Task.FromResult(0);
        }

        private async Task AnalyzeMarket(string symbol)
        {
            try
            {
                //Always get the freshest data
                var bMarket = await this.baseExchange.MarketSummary(symbol);
                var bBook = await this.baseExchange.OrderBook(symbol);
                var rMarket = await this.arbExchange.MarketSummary(symbol);
                var rBook = await this.arbExchange.OrderBook(symbol);

                if (bMarket == null || bBook == null || rMarket == null || rBook == null)
                {
                    dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, symbol, "AnalyzeMarkets", "Market Data Null", "");
                    Console.WriteLine("{0}: Null Market Data", symbol);
                }
                else
                {
                    await FindOpportunity(new ArbitrageMarket(bMarket, bBook, rMarket, rBook));
                    Console.WriteLine("Analyzing {0}", symbol);
                }
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, symbol, "AnalyzeMarkets", e.Message, e.StackTrace);
            }
        }

        public async Task FindOpportunity(ArbitrageMarket am)
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;

            baseBuy = EngineHelper.GetPriceAtVolumeThreshold(txThreshold, am.baseBook.asks);
            baseSell = EngineHelper.GetPriceAtVolumeThreshold(txThreshold, am.baseBook.bids);
            arbBuy = EngineHelper.GetPriceAtVolumeThreshold(txThreshold, am.arbitrageBook.asks);
            arbSell = EngineHelper.GetPriceAtVolumeThreshold(txThreshold, am.arbitrageBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (this.baseExchange.Fee + this.arbExchange.Fee);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (this.baseExchange.Fee + this.arbExchange.Fee);

                if (baseBuy < arbSell && baseBuySpread > 0 && runType == "trade")
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, this.runType, baseBuy, arbSell, baseBuySpread, txThreshold);
                    long buyId = await EngineHelper.Buy(baseExchange, baseExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold / baseBuy, baseBuy);
                    long sellId = await EngineHelper.Sell(arbExchange, arbitrageExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold / arbSell, arbSell);
                    dbService.SaveOrderPair(buyId, sellId);
                }

                if (baseSell > arbBuy && baseSellSpread >= 0 && runType == "trade")
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, this.runType, baseSell, arbBuy, baseSellSpread, txThreshold);
                    long buyId = await EngineHelper.Buy(arbExchange, arbitrageExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold / arbBuy, arbBuy);
                    long sellId = await EngineHelper.Sell(baseExchange, baseExchangeMarkets[am.Symbol], dbService, am.Symbol, txThreshold / baseSell, baseSell);
                    dbService.SaveOrderPair(buyId, sellId);
                }
            }
            await Task.FromResult(0);
        }
    }
}