using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;

namespace BMCore.Engine
{
    public class EngineHelper
    {
        public static void ExecuteAllExchanges(IExchange[] exchanges, BMDbService dbService, decimal threshold, string runType)
        {
            for (var i = 0; i < exchanges.Length; i++)
            {
                var baseExchange = exchanges[i];
                for (var j = 0; j < exchanges.Length; j++)
                {
                    var arbExchange = exchanges[j];

                    if (baseExchange != arbExchange)
                    {
                        ExecuteExchangePair(baseExchange, arbExchange, dbService, threshold, runType);
                    }

                }
            }
        }

        public static void ExecuteExchangePair(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, decimal threshold, string runType)
        {
            int pId = -1;
            try
            {
                Console.WriteLine("Starting: {0} {1}", baseExchange.Name, arbExchange.Name);
                pId = dbService.StartEngineProcess(baseExchange.Name, arbExchange.Name, runType);
                var engine = new TradingEngine(baseExchange, arbExchange, dbService, threshold);

                if (runType == "log")
                    engine.AnalyzeMarkets().Wait();
                else if (runType == "trade")
                    engine.AnalyzeFundedPairs().Wait();

                dbService.EndEngineProcess(pId, "success");
                Console.WriteLine("Completed: {0} {1}", baseExchange.Name, arbExchange.Name);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("Error: {0} {1}", baseExchange.Name, arbExchange.Name);
                dbService.LogError(baseExchange.Name, arbExchange.Name, "", "Main", e.Message, e.StackTrace);
                if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
            }
        }

        public static async Task<long> Sell(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price)
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "sell");
        }

        public static async Task<long> Buy(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price)
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "buy");
        }

        public static async Task<long> Trade(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, string side)
        {
            long orderId = dbService.InsertOrder(exchange.Name, symbol, market.BaseCurrency, market.MarketCurrency, side);
            var tradeResult = await exchange.Sell(orderId.ToString(), market.ExchangeSymbol, quantity, price);
            dbService.UpdateOrderUuid(orderId, tradeResult.Uuid);
            return orderId;
        }

        public static async Task UpdateWithdrawalStatus(IDbService dbService, Dictionary<string, IExchange> exchanges)
        {
            foreach (var withdrawal in dbService.GetWithdrawals(status: "pending"))
            {
                try
                {
                    var exchange = exchanges[withdrawal.FromExchange];
                    var w = await exchange.GetWithdrawal(withdrawal.Uuid);
                    if (!string.IsNullOrWhiteSpace(w.TxId))
                        dbService.CloseWithdrawal(withdrawal.Id, w.Amount, w.TxId);
                }
                catch (Exception ex)
                {
                    dbService.UpdateWithdrawalStatus(withdrawal.Id, "error", ex);
                    dbService.LogError(withdrawal.FromExchange, "", withdrawal.Uuid, "UpdateWithdrawalStatus", ex.Message, ex.StackTrace);

                }

            }
            await Task.FromResult(0);
        }

        public static async Task UpdateOrderStatus(IDbService dbService, Dictionary<string, IExchange> exchanges)
        {
            foreach (var order in dbService.GetOrders(status: "pending"))
            {
                try
                {
                    var exchange = exchanges[order.Exchange];
                    var o = await exchange.CheckOrder(order.Uuid);
                    if (o != null && o.IsFilled)
                        dbService.FillOrder(order.Id, o.Quantity, o.Price, o.Rate);
                }
                catch (Exception ex)
                {
                    dbService.UpdateOrderStatus(order.Id, "error", ex);
                    dbService.LogError(order.Exchange, order.ToExchange, order.Uuid, "ProcessWithdrawals", ex.Message, ex.StackTrace);

                }

            }
            await Task.FromResult(0);
        }

        public static async Task ProcessWithdrawals(IDbService dbService, Dictionary<string, IExchange> exchanges)
        {
            foreach (var order in dbService.GetOrders(status: "filled").Where(o => !string.IsNullOrWhiteSpace(o.ToExchange)))
            {
                try
                {
                    var fromExchange = exchanges[order.Exchange];
                    var toExchange = exchanges[order.ToExchange];
                    string address;
                    string currency;

                    if (order.Side == "buy")
                    {
                        var result = await toExchange.GetDepositAddress(order.MarketCurrency);
                        address = result.Address;
                        currency = result.Currency;
                    }
                    else
                    {
                        var result = await toExchange.GetDepositAddress(order.BaseCurrency);
                        address = result.Address;
                        currency = result.Currency;
                    }

                    var tx = await fromExchange.Withdraw(currency, order.Quantity, address);
                    dbService.InsertWithdrawal(tx.Uuid, order.Id, currency, fromExchange.Name, order.Quantity);
                }
                catch (Exception ex)
                {
                    dbService.UpdateOrderStatus(order.Id, "error", ex);
                    dbService.LogError(order.Exchange, order.ToExchange, order.Uuid, "ProcessWithdrawals", ex.Message, ex.StackTrace);

                }

            }
            await Task.FromResult(0);
        }

        public static decimal GetPriceAtVolumeThreshold(decimal threshold, List<OrderBookEntry> entries)
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