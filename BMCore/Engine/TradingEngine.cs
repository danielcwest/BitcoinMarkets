using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;

namespace BMCore.Engine
{
    public class TradingEngine
    {
        // BASE EXCHANGE
        IExchange baseExchange;
        Dictionary<string, ISymbol> baseExchangeMarkets;
        Dictionary<string, ICurrencyBalance> baseExchangeBalances;

        //ARBITRAGE EXCHANGE
        IExchange arbExchange;
        Dictionary<string, ISymbol> arbitrageExchangeMarkets;
        Dictionary<string, ICurrencyBalance> arbitrageExchangeBalances;
        BMDbService dbService;

        CurrencyConfig baseCurrency;

        string runType = "log";
        int pId = 0;

        public TradingEngine(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, CurrencyConfig baseCurrency, GmailConfig gmail, int pId)
        {
            this.dbService = dbService;
            this.baseExchange = baseExchange;
            this.arbExchange = arbExchange;
            this.baseCurrency = baseCurrency;
            this.pId = pId;
        }

        public async Task<int> AnalyzeFundedPairs()
        {
            this.runType = "trade";

            await RefreshSymbols();

            int count = 0;
            foreach (var kvp in this.baseExchangeBalances.Where(kvp => !EngineHelper.IsBaseCurrency(kvp.Value.Currency)))
            {
                if (this.arbitrageExchangeBalances.ContainsKey(kvp.Key))
                {
                    string symbol = string.Format("{0}{1}", kvp.Key, baseCurrency.Name);
                    bool valid = await AnalyzeMarket(symbol);
                    if (valid) count++;
                }
            }
            return count;
        }

        public async Task<int> AnalyzeMarkets()
        {
            this.runType = "log";

            await RefreshSymbols();

            int marketCount = 0;
            foreach (var kvp in this.baseExchangeMarkets)
            {
                if (this.arbitrageExchangeMarkets.ContainsKey(kvp.Key))
                {
                    bool valid = await AnalyzeMarket(kvp.Key);
                    if (valid) marketCount++;
                }
            }
            return marketCount;
        }

        private bool HasAvailableBalance(IMarket market)
        {
            bool result = false;
            if (this.baseExchangeBalances.ContainsKey(market.BaseCurrency) && this.baseExchangeBalances.ContainsKey(market.QuoteCurrency)
            && this.arbitrageExchangeBalances.ContainsKey(market.BaseCurrency) && this.arbitrageExchangeBalances.ContainsKey(market.QuoteCurrency))
            {
                result = this.baseExchangeBalances[market.BaseCurrency].Available > baseCurrency.TradeThreshold &&
                        this.baseExchangeBalances[market.QuoteCurrency].Available * market.Last > baseCurrency.TradeThreshold &&
                        this.arbitrageExchangeBalances[market.BaseCurrency].Available > baseCurrency.TradeThreshold &&
                        this.arbitrageExchangeBalances[market.QuoteCurrency].Available * market.Last > baseCurrency.TradeThreshold;
            }
            return result;
        }

        private async Task RefreshSymbols()
        {
            var baseMarkets = await this.baseExchange.Symbols();
            this.baseExchangeMarkets = baseMarkets.Where(m => m.BaseCurrency == baseCurrency.Name).ToDictionary(m => m.LocalSymbol);

            var arbExchange = await this.arbExchange.Symbols();
            this.arbitrageExchangeMarkets = arbExchange.Where(m => m.BaseCurrency == baseCurrency.Name).ToDictionary(m => m.LocalSymbol);

            if (this.runType == "trade")
            {
                var baseBalances = await this.baseExchange.GetBalances();
                this.baseExchangeBalances = baseBalances.Where(b => b.Available > 0).ToDictionary(b => b.Currency);

                var arbBalances = await this.arbExchange.GetBalances();
                this.arbitrageExchangeBalances = arbBalances.Where(b => b.Available > 0).ToDictionary(b => b.Currency);
            }


            await Task.FromResult(0);
        }

        private async Task<bool> AnalyzeMarket(string symbol)
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
                    dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, symbol, "AnalyzeMarkets", new Exception("No Market Data"), pId);
                    Console.WriteLine("{0}: Null Market Data", symbol);
                }
                else
                {
                    Console.WriteLine("Analyzing {0}", symbol);
                    if (runType == "trade" && HasAvailableBalance(bMarket) && HasAvailableBalance(rMarket))
                    {
                        await FindOpportunity(new ArbitrageMarket(bMarket, bBook, rMarket, rBook, this.baseCurrency));
                    }
                    else
                    {
                        await FindOpportunity(new ArbitrageMarket(bMarket, bBook, rMarket, rBook, this.baseCurrency));
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.arbExchange.Name, symbol, "AnalyzeMarkets", e, pId);
            }
            return false;
        }

        private async Task FindOpportunity(ArbitrageMarket am)
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;

            baseBuy = EngineHelper.GetPriceAtVolumeThreshold(baseCurrency.TradeThreshold, am.baseBook.asks);
            baseSell = EngineHelper.GetPriceAtVolumeThreshold(baseCurrency.TradeThreshold, am.baseBook.bids);
            arbBuy = EngineHelper.GetPriceAtVolumeThreshold(baseCurrency.TradeThreshold, am.arbitrageBook.asks);
            arbSell = EngineHelper.GetPriceAtVolumeThreshold(baseCurrency.TradeThreshold, am.arbitrageBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (this.baseExchange.Fee + this.arbExchange.Fee);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (this.baseExchange.Fee + this.arbExchange.Fee);

                //TODO: Domain and appsettings spread
                if (baseBuy < arbSell && baseBuySpread >= am.baseCurrency.SpreadThreshold && runType == "trade")
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, this.runType, baseBuy, arbSell, baseBuySpread, baseCurrency.TradeThreshold, this.pId);
                    long sellId = await EngineHelper.Sell(arbExchange, arbitrageExchangeMarkets[am.Symbol], dbService, am.Symbol, baseCurrency.TradeThreshold / arbSell, arbSell, this.pId);
                    long buyId = await EngineHelper.Buy(baseExchange, baseExchangeMarkets[am.Symbol], dbService, am.Symbol, baseCurrency.TradeThreshold / baseBuy, baseBuy, this.pId);
                    dbService.SaveOrderPair(buyId, baseExchange.Name, sellId, arbExchange.Name);
                }

                if (baseBuy < arbSell && baseBuySpread >= am.baseCurrency.SpreadThreshold && runType == "log")
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, this.runType, baseBuy, arbSell, baseBuySpread, baseCurrency.TradeThreshold, this.pId);
                }

                if (baseSell > arbBuy && baseSellSpread >= am.baseCurrency.SpreadThreshold && runType == "trade")
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, this.runType, baseSell, arbBuy, baseSellSpread, baseCurrency.TradeThreshold, this.pId);
                    long sellId = await EngineHelper.Sell(baseExchange, baseExchangeMarkets[am.Symbol], dbService, am.Symbol, baseCurrency.TradeThreshold / baseSell, baseSell, this.pId);
                    long buyId = await EngineHelper.Buy(arbExchange, arbitrageExchangeMarkets[am.Symbol], dbService, am.Symbol, baseCurrency.TradeThreshold / arbBuy, arbBuy, this.pId);
                    dbService.SaveOrderPair(buyId, arbExchange.Name, sellId, baseExchange.Name);
                }

                if (baseSell > arbBuy && baseSellSpread >= am.baseCurrency.SpreadThreshold && runType == "log")
                {
                    dbService.LogTrade(this.baseExchange.Name, this.arbExchange.Name, am.Symbol, this.runType, baseSell, arbBuy, baseSellSpread, baseCurrency.TradeThreshold, this.pId);
                }
            }
            await Task.FromResult(0);
        }
    }
}