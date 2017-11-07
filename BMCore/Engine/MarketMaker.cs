using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMCore.Engine
{
    public class MarketMaker
    {
        #region Public Entry Points
        IExchange baseExchange;
        IExchange counterExchange;
        IDbService dbService;

        GmailConfig gmail;
        EngineReporter reporter;

        public MarketMaker(IExchange baseExchange, IExchange counterExchange, IDbService dbService,  GmailConfig gmail)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
            this.gmail = gmail;

            this.reporter = new EngineReporter(baseExchange, counterExchange, dbService);
            
        }

        public async Task ResetLimitOrders(IEnumerable<ArbitragePair> pairs, int pId = -1)
        {
            await Task.WhenAll(pairs.Select(async p => await ResetLimitOrdersForPair(p, pId)));
        }

        public async Task ProcessOrders(IEnumerable<ArbitragePair> pairs)
        {
            foreach (var pair in pairs)
            {
                await ProcessOrdersForPair(pair);
            }
        }

        public async Task AuditCompletedOrders(IEnumerable<ArbitragePair> pairs)
        {
            var tasks = pairs.Select(async p => await AuditCompletedOrdersForPair(p));

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
                Console.WriteLine("Process Orders {0}", pair.Symbol);
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
                try
                {
                    var orders = await baseExchange.CancelOrders(pair.BaseSymbol);
                }
                catch (ApiException ae) when (ae.RequestMethod == HttpMethod.Delete)
                {
                    //ignore, orders expire after a minute anyway!  
                    Console.WriteLine("Ignoring Cancel Order Exception ... {0}", pair.Symbol);
                }

                await ProcessOpenOrdersForPair(pair);

                //Place new orders
                await PlaceLimitOrders(dbService, this.baseExchange, pair, pId);
            }
            catch (ApiException ae)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ResetLimitOrdersForPair", ae, pId);
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
            decimal arbBuy = EngineHelper.GetPriceAtVolumeThreshold(pair.BidMultiplier * pair.TradeThreshold, pair.counterBook.asks);
            decimal arbSell = EngineHelper.GetPriceAtVolumeThreshold(pair.AskMultiplier * pair.TradeThreshold, pair.counterBook.bids);

            //Set our BID at the price we can SELL on the counter exchange plus our commission
            decimal buyPriceRaw = arbSell - (arbSell * pair.BidSpread);
            decimal buyPriceRounded = Math.Round(buyPriceRaw, pair.DecimalPlaces);

            if(buyPriceRounded > buyPriceRaw)
            {
                buyPriceRounded -= pair.Increment;
            }

            //Set or ASK at the price we can BUY on counter plus our commission
            decimal sellPriceRaw = arbBuy + (arbBuy * pair.AskSpread);
            decimal sellPriceRounded = Math.Round(sellPriceRaw, pair.DecimalPlaces);

            if (sellPriceRounded < sellPriceRaw)
            {
                sellPriceRounded += pair.Increment;
            }

            decimal orderSpread = (sellPriceRounded - buyPriceRounded) / (sellPriceRounded + buyPriceRounded) - pair.CounterExchangeFee - pair.BaseExchangeFee;

            if (buyPriceRounded > 0 && orderSpread > 0)
            {
                int buyTxId = dbService.InsertMakerOrder(pair.Id, "buy", buyPriceRounded, pId);
                decimal buyQuantity = pair.BidMultiplier * pair.TradeThreshold / buyPriceRounded;
                var buyId = await EngineHelper.Buy(exchange, pair.GetBaseSymbol(), dbService, pair.Symbol, buyQuantity, buyPriceRounded, buyTxId);
                dbService.UpdateOrderUuid(buyTxId, buyId.Uuid);

            }

            if (sellPriceRounded > 0 && orderSpread > 0)
            {
                int sellTxId = dbService.InsertMakerOrder(pair.Id, "sell", sellPriceRounded, pId);
                decimal sellQuantity = pair.AskMultiplier * pair.TradeThreshold / sellPriceRounded;
                var sellId = await EngineHelper.Sell(exchange, pair.GetBaseSymbol(), dbService, pair.Symbol, sellQuantity, sellPriceRounded, sellTxId);
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

                try
                {
                    await EmailHelper.SendSimpleMailAsync(gmail, string.Format("Order Processed: {0}", order.BaseSymbol), string.Format("{0} - {1} {2} {3}", order.BaseExchange, order.CounterExchange, Environment.NewLine, order.Id));

                    await reporter.TakeBalanceSnapshot(order.ProcessId);
                }
                catch (Exception e)
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessCounterOrdersForPair", e, order.ProcessId);
                }

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

                    pair = await EngineHelper.AppendCounterMarketData(dbService, counterExchange, pair, includeMarket: false);

                    decimal buyPrice = EngineHelper.GetPriceAtVolumeThreshold(exchangeOrder.QuantityFilled, pair.counterBook.asks);
                    decimal buyQuantity = exchangeOrder.QuantityFilled + (exchangeOrder.QuantityFilled * order.CounterExchangeFee);

                    var counterOrder = await EngineHelper.Buy(counterExchange, pair.GetCounterSymbol(), dbService, pair.CounterSymbol, buyQuantity, buyPrice, order.Id, "market");

                    dbService.UpdateOrderUuid(order.Id, order.BaseOrderUuid, counterOrder.Uuid, buyPrice, "audit");
                    dbService.UpdateOrderStatus(order.Id, "audit", counterQuantityFilled: buyQuantity);
                }
                else if (order.Type == "buy")
                {
                    Console.WriteLine("Place Counter Sell {0}", pair.Symbol);

                    pair = await EngineHelper.AppendCounterMarketData(dbService, counterExchange, pair, includeMarket: false);

                    decimal sellPrice = EngineHelper.GetPriceAtVolumeThreshold(exchangeOrder.QuantityFilled, pair.counterBook.bids);
                    decimal sellQuantity = exchangeOrder.QuantityFilled + (exchangeOrder.QuantityFilled * order.CounterExchangeFee);

                    var counterOrder = await EngineHelper.Sell(counterExchange, pair.GetCounterSymbol(), dbService, pair.CounterSymbol, sellQuantity, sellPrice, order.Id, "market");

                    dbService.UpdateOrderUuid(order.Id, order.BaseOrderUuid, counterOrder.Uuid, sellPrice, "audit");
                    dbService.UpdateOrderStatus(order.Id, "audit", counterQuantityFilled: sellQuantity);
                }
            }
            catch (ApiException ae)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessCounterOrder", ae, pId);
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessCounterOrder", e, pId);
            }

        }

        private async Task ResetExchangeBalances(ArbitragePair pair)
        {

            await Task.FromResult(0);
        }


        private async Task AuditCompletedOrdersForPair(ArbitragePair pair)
        {
            foreach(var order in dbService.GetMakerOrdersByStatus("audit", pair.Id).Where(o => o.BaseExchange == baseExchange.Name && o.CounterExchange == counterExchange.Name))
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

                    dbService.UpdateOrder(order.Id, baseRate: baseOrder.AvgRate, counterRate: counterOrder.AvgRate, baseCost: baseOrder.CostProceeds, counterCost: counterOrder.CostProceeds, commission: commission, status: "complete", baseQuantityFilled: baseOrder.QuantityFilled, counterQuantityFilled: counterOrder.QuantityFilled);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "AuditCompletedOrdersForPair", e, -1);
                }
            }
        }
        #endregion
    }
}
