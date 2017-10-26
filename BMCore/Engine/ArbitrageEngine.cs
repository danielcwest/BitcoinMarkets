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

        public async Task LogOpportunities(IEnumerable<ArbitragePair> pairs, int pId)
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
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "LogOpportunities", e, pId);
                    isError = true;
                }
                finally
                {
                    dbService.UpdateArbitragePairById(pair.Id, isOpportunity: isOpportunity, isError: isError);
                }
            }
        }

        public async Task CheckBalances(IEnumerable<ArbitragePair> pairs, int pId)
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
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "CheckBalances", e, pId);
                    isError = true;
                }
                finally
                {
                    dbService.UpdateArbitragePairById(pair.Id, isError: isError, isFunded: IsFunded);
                }
            }
        }

        public async Task ExecuteTrades(IEnumerable<ArbitragePair> pairs, int pId)
        {
            foreach (var p in pairs)
            {
                Console.WriteLine("Trading {0}", p.Symbol);
                ArbitragePair pair = p;
                bool isTrade = false;
                bool isError = false;
                int txId = -1;
                object orderMetadata = new { };

                try
                {
                    pair = await AppendMarketData(p);
                    var opportunity = EngineHelper.FindOpportunity(pair);
                    if (opportunity != null && opportunity.Type == "basebuy")
                    {
                        txId = dbService.InsertTransaction(pair.Id, "basebuy");

                        decimal sellQuantity = pair.TradeThreshold / opportunity.CounterPrice;
                        var sellId = await EngineHelper.Sell(counterExchange, pair.GetCounterSymbol(), dbService, pair.Symbol, sellQuantity, opportunity.CounterPrice, txId);

                        //Amount to buy so when withdrawn will deposit the same amount on the outher exchange as was sold
                        decimal buyQuantity = sellQuantity + pair.MarketWithdrawalFee + pair.TradeThreshold * pair.ExchangeFees / 2;
                        var buyId = await EngineHelper.Buy(baseExchange, pair.GetBaseSymbol(), dbService, pair.Symbol, buyQuantity, opportunity.BasePrice, txId);

                        dbService.UpdateTransactionOrderUuid(txId, buyId.Uuid, sellId.Uuid);
                        isTrade = true;

                        orderMetadata = new { Type = "basebuy", SellQuantity = sellQuantity, BuyQuantity = buyQuantity, MarketWithdrawalFee = pair.MarketWithdrawalFee, TradeCommission = pair.TradeThreshold * pair.ExchangeFees };
                    }
                    else if (opportunity != null && opportunity.Type == "basesell")
                    {
                        txId = dbService.InsertTransaction(pair.Id, "basesell");

                        decimal sellQuantity = pair.TradeThreshold / opportunity.CounterPrice;
                        var sellId = await EngineHelper.Sell(baseExchange, pair.GetBaseSymbol(), dbService, pair.Symbol, sellQuantity, opportunity.BasePrice, txId);

                        decimal buyQuantity = sellQuantity + pair.MarketWithdrawalFee + pair.TradeThreshold * pair.ExchangeFees / 2;
                        var buyId = await EngineHelper.Buy(counterExchange, pair.GetCounterSymbol(), dbService, pair.Symbol, buyQuantity, opportunity.CounterPrice, txId);

                        dbService.UpdateTransactionOrderUuid(txId, sellId.Uuid, buyId.Uuid);
                        isTrade = true;

                        orderMetadata = new { Type = "basesell", SellQuantity = sellQuantity, BuyQuantity = buyQuantity, MarketWithdrawalFee = pair.MarketWithdrawalFee, TradeCommission = pair.TradeThreshold * pair.ExchangeFees };
                    }
                }
                catch (Exception e)
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ExecuteTrades", e, pId);
                    isError = true;
                }
                finally
                {
                    dbService.UpdateArbitragePairById(pair.Id, isTrade: isTrade, isError: isError, isFunded: true, payload: orderMetadata);
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