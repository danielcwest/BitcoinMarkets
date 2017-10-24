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
    public class ArbitrageEngine
    {
        IExchange baseExchange;
        IExchange counterExchange;
        BMDbService dbService;

        Dictionary<string, ICurrencyBalance> baseExchangeBalances;
        Dictionary<string, ICurrencyBalance> counterExchangeBalances;


        public ArbitrageEngine(IExchange baseExchange, IExchange counterExchange, BMDbService dbService)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
        }

        public async Task LogOpportunities(IEnumerable<ArbitragePair> pairs)
        {
            foreach (var p in pairs)
            {
                Console.WriteLine("Logging {0}", p.Symbol);
                ArbitragePair pair = p;
                bool isOpportunity = false;
                bool isError = false;
                try
                {
                    pair = await AppendMarketData(p);
                    var opp = EngineHelper.FindOpportunity(pair);

                    if (opp != null)
                    {
                        dbService.InsertArbitrageOpportunity(pair.Id, opp.BasePrice, opp.CounterPrice, opp.Spread, pair.TradeThreshold);
                        isOpportunity = true;
                    }
                }
                catch (Exception e)
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "LogOpportunities", e, pair.Id);
                    isError = true;
                }
                finally
                {
                    dbService.UpdateArbitragePairById(pair.Id, isOpportunity: isOpportunity, isError: isError);
                }
            }
        }

        public async Task CheckBalances(IEnumerable<ArbitragePair> pairs)
        {
            await this.RefreshBalances();

            foreach (var p in pairs)
            {
                Console.WriteLine("Check Balances {0}", p.Symbol);
                ArbitragePair pair = p;
                bool IsFunded = false;
                bool isError = false;
                try
                {
                    pair = await AppendMarketData(p);
                    IsFunded = HasAvailableBalance(pair);
                }
                catch (Exception e)
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "CheckBalances", e, pair.Id);
                    isError = true;
                }
                finally
                {
                    dbService.UpdateArbitragePairById(pair.Id, isError: isError, isFunded: IsFunded);
                }
            }
        }

        public async Task ExecuteTrades(IEnumerable<ArbitragePair> pairs)
        {
            foreach (var p in pairs)
            {
                Console.WriteLine("Trading {0}", p.Symbol);
                ArbitragePair pair = p;
                bool isTrade = false;
                bool isError = false;
                try
                {
                    pair = await AppendMarketData(p);
                    var opp = EngineHelper.FindOpportunity(pair);

                    if (opp != null && opp.Type == "basebuy")
                    {
                        dbService.InsertArbitrageOpportunity(pair.Id, opp.BasePrice, opp.CounterPrice, opp.Spread, pair.TradeThreshold);
                        long buyId = await EngineHelper.Buy(baseExchange, pair.GetBaseSymbol(), dbService, pair.Symbol, pair.TradeThreshold / opp.BasePrice, opp.BasePrice, pair.Id);
                        long sellId = await EngineHelper.Sell(counterExchange, pair.GetCounterSymbol(), dbService, pair.Symbol, pair.TradeThreshold / opp.CounterPrice, opp.CounterPrice, pair.Id);
                        dbService.SaveOrderPair(buyId, baseExchange.Name, sellId, counterExchange.Name);
                        isTrade = true;

                    }
                    else if (opp != null && opp.Type == "basesell")
                    {
                        dbService.InsertArbitrageOpportunity(pair.Id, opp.BasePrice, opp.CounterPrice, opp.Spread, pair.TradeThreshold);
                        long buyId = await EngineHelper.Buy(counterExchange, pair.GetCounterSymbol(), dbService, pair.Symbol, pair.TradeThreshold / opp.CounterPrice, opp.CounterPrice, pair.Id);
                        long sellId = await EngineHelper.Sell(baseExchange, pair.GetBaseSymbol(), dbService, pair.Symbol, pair.TradeThreshold / opp.BasePrice, opp.BasePrice, pair.Id);
                        dbService.SaveOrderPair(buyId, counterExchange.Name, sellId, baseExchange.Name);
                        isTrade = true;
                    }
                }
                catch (Exception e)
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ExecuteTrades", e, pair.Id);
                    isError = true;
                }
                finally
                {
                    dbService.UpdateArbitragePairById(pair.Id, isTrade: isTrade, isError: isError);
                }
            }
        }
        private async Task<ArbitragePair> AppendMarketData(ArbitragePair pair)
        {
            //Always get the freshest data
            pair.baseMarket = await this.baseExchange.MarketSummary(pair.Symbol);
            pair.baseBook = await this.baseExchange.OrderBook(pair.Symbol);
            pair.counterMarket = await this.counterExchange.MarketSummary(pair.Symbol);
            pair.counterBook = await this.counterExchange.OrderBook(pair.Symbol);

            if (pair.baseMarket == null || pair.baseBook == null || pair.counterMarket == null || pair.counterBook == null)
            {
                var ex = new Exception("No Market Data");
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "AnalyzeMarket", ex, pair.Id);
                Console.WriteLine("{0}: Null Market Data", pair.Symbol);
                throw ex;
            }

            return pair;
        }

        private bool HasAvailableBalance(ArbitragePair market)
        {
            bool result = false;
            if (this.baseExchangeBalances.ContainsKey(market.BaseCurrency) && this.baseExchangeBalances.ContainsKey(market.MarketCurrency)
            && this.counterExchangeBalances.ContainsKey(market.BaseCurrency) && this.counterExchangeBalances.ContainsKey(market.MarketCurrency))
            {
                result = this.baseExchangeBalances[market.BaseCurrency].Available > market.TradeThreshold &&
                        this.baseExchangeBalances[market.MarketCurrency].Available * market.baseMarket.Last > market.TradeThreshold &&
                        this.counterExchangeBalances[market.BaseCurrency].Available > market.TradeThreshold &&
                        this.counterExchangeBalances[market.MarketCurrency].Available * market.counterMarket.Last > market.TradeThreshold;
            }
            return result;
        }

        private async Task RefreshBalances()
        {
            var baseBalances = await this.baseExchange.GetBalances();
            this.baseExchangeBalances = baseBalances.Where(b => b.Available > 0).ToDictionary(b => b.Currency);

            var arbBalances = await this.counterExchange.GetBalances();
            this.counterExchangeBalances = arbBalances.Where(b => b.Available > 0).ToDictionary(b => b.Currency);

            await Task.FromResult(0);
        }
    }
}