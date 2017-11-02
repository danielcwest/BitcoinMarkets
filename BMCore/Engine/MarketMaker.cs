using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCore.Engine
{
    public class MarketMaker
    {
        #region Public Entry Points
        IExchange baseExchange;
        IExchange counterExchange;
        IDbService dbService;
        EngineReporter reporter;

        public MarketMaker(IExchange baseExchange, IExchange counterExchange, IDbService dbService)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
            this.reporter = new EngineReporter(baseExchange, counterExchange, dbService);
        }

        public async Task ResetLimitOrders(int pId)
        {
            var pairs = dbService.GetArbitragePairs("market").Select(p => new ArbitragePair(p));
            await ResetLimitOrders(pairs, pId);
        }

        public async Task ResetLimitOrders(IEnumerable<ArbitragePair> pairs, int pId)
        {
            var tasks = pairs.Select(async p => await ResetLimitOrdersForPair(p, pId));
            await Task.WhenAll(tasks);
        }

        public async Task LogMarketMakingPairs(int pId)
        {
            var pairs = dbService.GetArbitragePairs("market").Select(p => new ArbitragePair(p));
            await LogMarketMakingPairs(pairs, pId);
        }

        public async Task LogMarketMakingPairs(IEnumerable<ArbitragePair> pairs, int pId)
        {
            var tasks = pairs.Select(async p => await LogMarketMakingPair(p, pId));

            await Task.WhenAll(tasks);
        }

        public async Task ProcessOrders()
        {
            var pairs = dbService.GetArbitragePairs("market").Select(p => new ArbitragePair(p));
            await ProcessOrders(pairs);
        }

        public async Task ProcessOrders(IEnumerable<ArbitragePair> pairs)
        {
            var tasks = pairs.Select(async p => await ProcessOrdersForPair(p));

            await Task.WhenAll(tasks);
        }

        public async Task AuditCompletedOrders(int pId)
        {
            var pairs = dbService.GetArbitragePairs("market").Select(p => new ArbitragePair(p));
            await AuditCompletedOrders(pairs, pId);
        }

        public async Task AuditCompletedOrders(IEnumerable<ArbitragePair> pairs, int pId)
        {
            var tasks = pairs.Select(async p => await AuditCompletedOrdersForPair(p, pId));

            await Task.WhenAll(tasks);
        }
        #endregion


        #region Logging
        private async Task LogMarketMakingPair(ArbitragePair pair, int pId)
        {

            bool isTrade = false;
            bool isError = false;

            try
            {
                Console.WriteLine("Market Make {0}", pair.Symbol);

                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair, false);

                decimal baseBuy = 0M;
                decimal baseSell = 0M;
                decimal arbBuy = 0M;
                decimal arbSell = 0M;

                arbBuy = EngineHelper.GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.asks);
                arbSell = EngineHelper.GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.bids);
                baseBuy = arbBuy - (arbBuy * pair.BidSpread);
                baseSell = arbSell + (arbSell * pair.AskSpread);

                decimal orderSpread = pair.BidSpread - pair.CounterExchangeFee;

                dbService.InsertArbitrageOpportunity(pair.Id, baseBuy, baseSell, orderSpread, pair.TradeThreshold);
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ExecuteTrades", e, pId);
                isError = true;
            }
            finally
            {
               dbService.UpdateArbitragePairById(pair.Id, isTrade: isTrade, isError: isError);
            }
        }

        #endregion


        #region Trading

        private async Task ProcessOrdersForPair(ArbitragePair pair)
        {
            bool isError = false;
            try
            {
                Console.WriteLine("Process Counter Orders {0}", pair.Symbol);
                await ProcessCounterOrdersForPair(pair);
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessOrdersForPair", e, pair.Id);
                isError = true;
            }
            finally
            {
                dbService.UpdateArbitragePairById(pair.Id, isTrade: false, isError: isError, isFunded: true);
            }
        }

        private async Task ResetLimitOrdersForPair(ArbitragePair pair, int pId)
        {
            bool isError = false;

            try
            {
                Console.WriteLine("Reset Limit Orders {0}", pair.Symbol);

                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair, includeBaseMarket: false, includeBaseBook: false, includeCounterMarket: false, includeCounterBook: true);

                //Cancel existing orders
                var orders = await baseExchange.CancelOrders(pair.BaseSymbol);

                await ProcessOpenOrdersForPair(pair);

                //Place new orders
                await PlaceLimitOrders(dbService, this.baseExchange, pair, pId);
            }
            catch (Exception e)
            {
                await baseExchange.CancelOrders(pair.BaseSymbol);
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ResetLimitOrdersForPair", e, pId);
                isError = true;
            }
            finally
            {
                dbService.UpdateArbitragePairById(pair.Id, isTrade: false, isError: isError, isFunded: true);
            }
        }

        private static async Task PlaceLimitOrders(IDbService dbService, IExchange exchange, ArbitragePair pair, int pId)
        {

            decimal arbBuy = EngineHelper.GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.asks);
            decimal arbSell = EngineHelper.GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.bids);
            decimal buyPrice = arbBuy - (arbBuy * pair.BidSpread);
            decimal sellPrice = arbSell + (arbSell * pair.AskSpread);

            decimal orderSpread = (sellPrice - buyPrice) / (sellPrice + buyPrice) - pair.CounterExchangeFee;

            dbService.InsertArbitrageOpportunity(pair.Id, buyPrice, sellPrice, orderSpread, pair.TradeThreshold);

            Console.WriteLine("Placing Limit Orders With Spread {0:P4}", pair.BidSpread + pair.AskSpread);

            if (buyPrice > 0)
            {
                int buyTxId = dbService.InsertMakerOrder(pair.Id, "buy", buyPrice, pId);
                decimal buyQuantity = pair.TradeThreshold / buyPrice;
                var buyId = await EngineHelper.Buy(exchange, pair.GetBaseSymbol(), dbService, pair.Symbol, buyQuantity, buyPrice, buyTxId);
                dbService.UpdateOrderUuid(buyTxId, buyId.Uuid);

            }

            if (sellPrice > 0)
            {
                int sellTxId = dbService.InsertMakerOrder(pair.Id, "sell", sellPrice, pId);
                decimal sellQuantity = pair.TradeThreshold / sellPrice;
                var sellId = await EngineHelper.Sell(exchange, pair.GetBaseSymbol(), dbService, pair.Symbol, sellQuantity, sellPrice, sellTxId);
                dbService.UpdateOrderUuid(sellTxId, sellId.Uuid);

            }

        }

        private async Task ProcessOpenOrdersForPair(ArbitragePair pair)
        {
            var orders = dbService.GetMakerOrdersByStatus("open", pair.Id);
            var tasks = orders.AsParallel().WithDegreeOfParallelism(2).Select(async o => await ProcessOpenOrder(o, pair));
            await Task.WhenAll(tasks);
        }

        private async Task ProcessOpenOrder(DbMakerOrder order, ArbitragePair pair)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(order.BaseOrderUuid))
                {
                    dbService.UpdateOrderStatus(order.Id, "closed");
                }
                else
                {
                    var exchangeOrder = await baseExchange.CheckOrder(order.BaseOrderUuid);

                    if (exchangeOrder.QuantityFilled > 0)
                    {
                        Console.WriteLine("{2} Order traded {0} / {1}", exchangeOrder.QuantityFilled, exchangeOrder.Quantity, pair.Symbol);
                        dbService.UpdateOrderStatus(order.Id, "pending", baseQuantityFilled: exchangeOrder.QuantityFilled);
                    }
                    else
                    {
                        //Close MakerOrder
                        dbService.UpdateOrderStatus(order.Id, "closed");
                    }
                }
            }
            catch(ApiException ae)
            {
                if(ae.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    dbService.UpdateOrderStatus(order.Id, "closed");
                }
                else
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessOpenOrder", ae, order.ProcessId);
                    dbService.UpdateOrderStatus(order.Id, "error");
                }
            }
            catch (Exception e)
            {
                dbService.UpdateOrderStatus(order.Id, "error");
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessOpenOrder", e, order.ProcessId);
            }

        }

        private async Task ProcessCounterOrdersForPair(ArbitragePair pair)
        {
            foreach(var order in dbService.GetMakerOrdersByStatus("pending", pair.Id))
            {
                await ProcessCounterOrder(pair, order, order.ProcessId);
                await reporter.TakeBalanceSnapshot(order.ProcessId);
            }
        }

        private async Task ProcessCounterOrder(ArbitragePair pair, DbMakerOrder order, int pId)
        {
            try
            {
                var exchangeOrder = await baseExchange.CheckOrder(order.BaseOrderUuid);

                if (order.Type == "sell")
                {
                    Console.WriteLine("Place Counter Buy {0}", pair.Symbol);

                    pair = await EngineHelper.AppendCounterMarketData(dbService, counterExchange, pair);

                    decimal buyPrice = EngineHelper.GetPriceAtVolumeThreshold(exchangeOrder.QuantityFilled, pair.counterBook.asks);
                    decimal buyQuantity = exchangeOrder.QuantityFilled + (exchangeOrder.QuantityFilled * order.CounterExchangeFee);

                    var counterOrder = await EngineHelper.Buy(counterExchange, pair.GetCounterSymbol(), dbService, pair.CounterSymbol, buyQuantity, buyPrice, order.Id, "market");

                    dbService.UpdateOrderUuid(order.Id, order.BaseOrderUuid, counterOrder.Uuid, buyPrice, "audit");
                    dbService.UpdateOrderStatus(order.Id, "audit", counterQuantityFilled: buyQuantity);
                }
                else if (order.Type == "buy")
                {
                    Console.WriteLine("Place Counter Sell {0}", pair.Symbol);

                    pair = await EngineHelper.AppendCounterMarketData(dbService, counterExchange, pair);

                    decimal sellPrice = EngineHelper.GetPriceAtVolumeThreshold(exchangeOrder.QuantityFilled, pair.counterBook.bids);
                    decimal sellQuantity = exchangeOrder.QuantityFilled + order.CounterBaseWithdrawalFee / sellPrice;

                    var counterOrder = await EngineHelper.Sell(counterExchange, pair.GetCounterSymbol(), dbService, pair.CounterSymbol, sellQuantity, sellPrice, order.Id, "market");

                    dbService.UpdateOrderUuid(order.Id, order.BaseOrderUuid, counterOrder.Uuid, sellPrice, "audit");
                    dbService.UpdateOrderStatus(order.Id, "audit", counterQuantityFilled: sellQuantity);
                }
            }
            catch(Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessCounterOrder", e, pId);
            }

        }

        private async Task ResetExchangeBalances(ArbitragePair pair)
        {

            await Task.FromResult(0);
        }


        private async Task AuditCompletedOrdersForPair(ArbitragePair pair, int pId)
        {
            foreach(var order in dbService.GetMakerOrdersByStatus("audit", pair.Id))
            {
                try
                {
                    var baseOrder = await baseExchange.CheckOrder(order.BaseOrderUuid);
                    var counterOrder = await counterExchange.CheckOrder(order.CounterOrderUuid);

                    decimal commission = 0m;
                    if (baseOrder.Side == "buy")
                    {
                        commission = counterOrder.CostProceeds - baseOrder.CostProceeds;
                    }
                    else
                    {
                        commission = baseOrder.CostProceeds - counterOrder.CostProceeds;
                    }

                    dbService.UpdateOrder(order.Id, baseRate: baseOrder.AvgRate, counterRate: counterOrder.AvgRate, baseCost: baseOrder.CostProceeds, counterCost: counterOrder.CostProceeds, commission: commission, status: "complete");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        #endregion
    }
}
