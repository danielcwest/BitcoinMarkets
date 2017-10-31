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
            var tasks = pairs.AsParallel().WithDegreeOfParallelism(4).Select(async p => await LogOpportunity(p, pId));

            await Task.WhenAll(tasks);
        }

        private async Task LogOpportunity(ArbitragePair pair, int pId)
        {
            Console.WriteLine("Logging {0}", pair.Symbol);
            bool isOpportunity = false;
            bool isError = false;
            try
            {
                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair);
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

        public async Task CheckBalances(IEnumerable<ArbitragePair> pairs, int pId)
        {
            await this.RefreshBalances();

            var tasks = pairs.AsParallel().WithDegreeOfParallelism(4).Select(async p => await CheckBalance(p, pId));

            await Task.WhenAll(tasks);
        }

        private async Task CheckBalance(ArbitragePair pair, int pId)
        {
            Console.WriteLine("Check Balances {0}", pair.Symbol);
            bool IsFunded = false;
            bool isError = false;
            try
            {
                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair);
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

        public async Task ExecuteTrades(IEnumerable<ArbitragePair> pairs, int pId)
        {
            var tasks = pairs.AsParallel().WithDegreeOfParallelism(4).Select(async p => await ExecutePair(p, pId));

            await Task.WhenAll(tasks);
        }

        private async Task ExecutePair(ArbitragePair pair, int pId)
        {

            bool isTrade = false;
            bool isError = false;
            int txId = -1;

            try
            {
                Console.WriteLine("Trade {0}", pair.Symbol);

                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair);

                var opportunity = EngineHelper.FindOpportunity(pair);
                if (opportunity != null && opportunity.Type == "basebuy")
                {
                    txId = dbService.InsertTransaction(pair.Id, "basebuy");

                    decimal sellQuantity = pair.TradeThreshold / opportunity.CounterPrice;
                    var sellId = await EngineHelper.Sell(counterExchange, pair.GetCounterSymbol(), dbService, pair.Symbol, sellQuantity, opportunity.CounterPrice, txId);

                    //Amount to buy so when withdrawn will deposit the same amount on the other exchange as was sold
                    decimal buyQuantity = sellQuantity + pair.BaseMarketWithdrawalFee;
                    var buyId = await EngineHelper.Buy(baseExchange, pair.GetBaseSymbol(), dbService, pair.Symbol, buyQuantity, opportunity.BasePrice, txId);

                    dbService.UpdateTransactionOrderUuid(txId, buyId.Uuid, sellId.Uuid, new { Type = "basebuy", BuyPrice = opportunity.BasePrice, SellPrice = opportunity.CounterPrice, Spread = opportunity.Spread });
                    isTrade = true;

                }
                else if (opportunity != null && opportunity.Type == "basesell")
                {
                    txId = dbService.InsertTransaction(pair.Id, "basesell");

                    decimal sellQuantity = pair.TradeThreshold / opportunity.BasePrice;
                    var sellId = await EngineHelper.Sell(baseExchange, pair.GetBaseSymbol(), dbService, pair.Symbol, sellQuantity, opportunity.BasePrice, txId);

                    decimal buyQuantity = sellQuantity + pair.CounterMarketWithdrawalFee;
                    var buyId = await EngineHelper.Buy(counterExchange, pair.GetCounterSymbol(), dbService, pair.Symbol, buyQuantity, opportunity.CounterPrice, txId);

                    dbService.UpdateTransactionOrderUuid(txId, sellId.Uuid, buyId.Uuid, new { Type = "basesell", BuyPrice = opportunity.CounterPrice, SellPrice = opportunity.BasePrice, Spread = opportunity.Spread });
                    isTrade = true;
                }
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ExecuteTrades", e, pId);
                isError = true;
            }
            finally
            {
                dbService.UpdateArbitragePairById(pair.Id, isTrade: isTrade, isError: isError, isFunded: true);
            }
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