using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;
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
        BMDbService dbService;

        public MarketMaker(IExchange baseExchange, IExchange counterExchange, BMDbService dbService)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
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
            var pairs = dbService.GetArbitragePairs("log").Select(p => new ArbitragePair(p));
            await LogMarketMakingPairs(pairs, pId);
        }

        public async Task LogMarketMakingPairs(IEnumerable<ArbitragePair> pairs, int pId)
        {
            var tasks = pairs.Select(async p => await LogMarketMakingPair(p, pId));

            await Task.WhenAll(tasks);
        }

        public async Task ProcessOrders(int pId)
        {
            var pairs = dbService.GetArbitragePairs("market").Select(p => new ArbitragePair(p));
            await ProcessOrders(pairs, pId);
        }

        public async Task ProcessOrders(IEnumerable<ArbitragePair> pairs, int pId)
        {
            var tasks = pairs.Select(async p => await ProcessOrdersForPair(p, pId));

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

                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair);

                decimal baseBuy = 0M;
                decimal baseSell = 0M;
                decimal arbBuy = 0M;
                decimal arbSell = 0M;
                decimal baseBuySpread = 0M;
                decimal baseSellSpread = 0M;

                baseBuy = EngineHelper.GetPriceAtVolumeThreshold(pair.MarketThreshold, pair.baseBook.asks);
                baseSell = EngineHelper.GetPriceAtVolumeThreshold(pair.MarketThreshold, pair.baseBook.bids);
                arbBuy = EngineHelper.GetPriceAtVolumeThreshold(pair.MarketThreshold, pair.counterBook.asks);
                arbSell = EngineHelper.GetPriceAtVolumeThreshold(pair.MarketThreshold, pair.counterBook.bids);

                if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
                {
                    baseBuySpread = ((arbSell - baseBuy) / arbSell) - (pair.BaseExchangeFee + pair.CounterExchangeFee - pair.MarketSpread / 2);
                    dbService.InsertArbitrageOpportunity(pair.Id, baseBuy, arbSell, baseBuySpread, pair.MarketThreshold);

                    baseSellSpread = ((baseSell - arbBuy) / baseSell) - (pair.BaseExchangeFee + pair.CounterExchangeFee - pair.MarketSpread / 2);
                    dbService.InsertArbitrageOpportunity(pair.Id, baseSell, arbBuy, baseSellSpread, pair.MarketThreshold);
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

        #endregion


        #region Trading

        private async Task ProcessOrdersForPair(ArbitragePair pair, int pId)
        {
            bool isError = false;
            try
            {
                Console.WriteLine("Process Counter Orders {0}", pair.Symbol);
                await ProcessCounterOrdersForPair(pair, pId);
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessOrdersForPair", e, pId);
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

                pair = await EngineHelper.AppendMarketData(dbService, this.baseExchange, this.counterExchange, pair);

                //Cancel existing orders
                var orders = await baseExchange.CancelOrders(pair.BaseSymbol);

                await ProcessOpenOrdersForPair(pair, pId);

                //Place new orders
                await PlaceLimitOrders(dbService, this.baseExchange, pair);
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

        private static async Task PlaceLimitOrders(IDbService dbService, IExchange exchange, ArbitragePair pair)
        {
            decimal price = (pair.baseMarket.Ask + pair.baseMarket.Bid) / 2;
            Console.WriteLine("Last: {0:N8} Price: {1:N8}", pair.baseMarket.Last, price);
            decimal buyPrice = price - (price * pair.MarketSpread / 2);
            decimal sellPrice = price + (price * pair.MarketSpread / 2);

            decimal spread = 2 * (sellPrice - buyPrice) / (sellPrice + buyPrice);

            Console.WriteLine("Placing Limit Orders With Spread {0:P4}", spread);

            int buyTxId = dbService.InsertMakerOrder(pair.Id, "buy", buyPrice);
            decimal buyQuantity = pair.MarketThreshold / buyPrice;
            var buyId = await EngineHelper.Buy(exchange, pair.GetBaseSymbol(), dbService, pair.Symbol, buyQuantity, buyPrice, buyTxId);

            int sellTxId = dbService.InsertMakerOrder(pair.Id, "sell", sellPrice);
            decimal sellQuantity = pair.MarketThreshold / sellPrice;
            var sellId = await EngineHelper.Sell(exchange, pair.GetBaseSymbol(), dbService, pair.Symbol, sellQuantity, sellPrice, sellTxId);

            dbService.UpdateOrderUuid(buyTxId, buyId.Uuid);
            dbService.UpdateOrderUuid(sellTxId, sellId.Uuid);
        }

        private async Task ProcessOpenOrdersForPair(ArbitragePair pair, int pId)
        {
            var orders = dbService.GetMakerOrdersByStatus("open", pair.Id);
            var tasks = orders.AsParallel().WithDegreeOfParallelism(2).Select(async o => await ProcessOpenOrder(o, pair, pId));
            await Task.WhenAll(tasks);
        }

        private async Task ProcessOpenOrder(DbMakerOrder order, ArbitragePair pair, int pId)
        {
            try
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
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, pair.Symbol, "ProcessOpenOrder", e, pId);
            }

        }

        private async Task ProcessCounterOrdersForPair(ArbitragePair pair, int pId)
        {
            foreach(var order in dbService.GetMakerOrdersByStatus("pending", pair.Id))
            {
                await ProcessCounterOrder(pair, order, pId);
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

                    var counterOrder = await EngineHelper.Buy(counterExchange, pair.GetCounterSymbol(), dbService, pair.CounterSymbol, buyQuantity, buyPrice, order.Id);

                    dbService.UpdateOrderUuid(order.Id, order.BaseOrderUuid, counterOrder.Uuid, buyPrice, "complete");
                    dbService.UpdateOrderStatus(order.Id, "complete", counterQuantityFilled: buyQuantity);
                }
                else if (order.Type == "buy")
                {
                    Console.WriteLine("Place Counter Sell {0}", pair.Symbol);

                    pair = await EngineHelper.AppendCounterMarketData(dbService, counterExchange, pair);

                    decimal sellPrice = EngineHelper.GetPriceAtVolumeThreshold(exchangeOrder.QuantityFilled, pair.counterBook.bids);
                    decimal sellQuantity = exchangeOrder.QuantityFilled + order.CounterBaseWithdrawalFee / sellPrice;

                    var counterOrder = await EngineHelper.Sell(counterExchange, pair.GetCounterSymbol(), dbService, pair.CounterSymbol, sellQuantity, sellPrice, order.Id);

                    dbService.UpdateOrderUuid(order.Id, order.BaseOrderUuid, counterOrder.Uuid, sellPrice, "complete");
                    dbService.UpdateOrderStatus(order.Id, "complete", counterQuantityFilled: sellQuantity);
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


        #endregion
    }
}
