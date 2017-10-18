using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;

namespace BMCore.Engine
{
    public class EngineHelper
    {
        public static void ExecuteAllExchanges(IExchange[] exchanges, BMDbService dbService, CurrencyConfig baseCurrency, GmailConfig gmail, string runType)
        {
            for (var i = 0; i < exchanges.Length; i++)
            {
                var baseExchange = exchanges[i];
                for (var j = 0; j < exchanges.Length; j++)
                {
                    var arbExchange = exchanges[j];

                    if (baseExchange != arbExchange)
                    {
                        ExecuteExchangePair(baseExchange, arbExchange, dbService, baseCurrency, gmail, runType);
                    }

                }
            }
        }

        public static void ExecuteExchangePair(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, CurrencyConfig baseCurrency, GmailConfig gmail, string runType)
        {
            int pId = -1;
            try
            {
                Console.WriteLine("Starting: {0} {1}", baseExchange.Name, arbExchange.Name);
                pId = dbService.StartEngineProcess(baseExchange.Name, arbExchange.Name, runType, baseCurrency);
                var engine = new TradingEngine(baseExchange, arbExchange, dbService, baseCurrency, gmail);

                int count = 0;
                if (runType == "log")
                    count = engine.AnalyzeMarkets().Result;
                else if (runType == "trade")
                    count = engine.AnalyzeFundedPairs(pId).Result;

                dbService.EndEngineProcess(pId, "success", new { MarketCount = count });
                Console.WriteLine("Completed: {0} {1}", baseExchange.Name, arbExchange.Name);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("Error: {0} {1}", baseExchange.Name, arbExchange.Name);
                dbService.LogError(baseExchange.Name, arbExchange.Name, "", "Main", e, pId);
                if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
            }
        }

        public static async Task<long> Sell(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, int pId)
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "sell", pId);
        }

        public static async Task<long> Buy(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, int pId)
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "buy", pId);
        }

        public static async Task<long> Trade(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, string side, int pId)
        {
            long orderId = dbService.InsertOrder(exchange.Name, symbol, market.BaseCurrency, market.MarketCurrency, side, pId, price);
            IAcceptedAction tradeResult;
            if (side == "buy")
            {
                tradeResult = await exchange.Buy(orderId.ToString(), market.ExchangeSymbol, quantity, price);
            }
            else
            {
                tradeResult = await exchange.Sell(orderId.ToString(), market.ExchangeSymbol, quantity, price);
            }
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
                    //dbService.UpdateWithdrawalStatus(withdrawal.Id, "error", ex);
                    dbService.LogError(withdrawal.FromExchange, "", withdrawal.Uuid, "UpdateWithdrawalStatus", ex);

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
                    dbService.LogError(order.Exchange, "", order.Uuid, "UpdateOrderStatus", ex);

                }

            }
            await Task.FromResult(0);
        }

        public static async Task ProcessWithdrawals(IDbService dbService, IExchange[] exchanges, CurrencyConfig baseCurrency)
        {
            for (var i = 0; i < exchanges.Length; i++)
            {
                var baseExchange = exchanges[i];
                for (var j = 0; j < exchanges.Length; j++)
                {
                    var arbExchange = exchanges[j];

                    if (baseExchange != arbExchange)
                    {
                        await ProcessWithdrawOrders(baseExchange, arbExchange, dbService, baseCurrency.TradeThreshold, "buy");
                        await ProcessWithdrawOrders(baseExchange, arbExchange, dbService, baseCurrency.TradeThreshold, "sell");
                    }

                }
            }
            await Task.FromResult(0);
        }

        private static async Task ProcessWithdrawOrders(IExchange from, IExchange to, IDbService dbService, decimal threshold, string side)
        {
            var ordersDic = dbService.GetOrdersToWithdraw(from.Name, to.Name, side, threshold).GroupBy(g => g.Currency).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var kvp in ordersDic)
            {
                try
                {
                    var depAddress = await to.GetDepositAddress(kvp.Key);

                    decimal quantity = kvp.Value.Sum(o => o.Quantity);

                    string symbol = kvp.Value.FirstOrDefault().Symbol;

                    if (quantity > 0 && !string.IsNullOrWhiteSpace(depAddress.Address) && !string.IsNullOrWhiteSpace(kvp.Key))
                    {
                        var tx = await from.Withdraw(kvp.Key, quantity, depAddress.Address);
                        dbService.InsertWithdrawal(tx.Uuid, 0, kvp.Key, from.Name, quantity, 0);
                        dbService.CloseOrders(kvp.Value.Select(o => o.Id));
                    }
                }
                catch (Exception ex)
                {
                    dbService.LogError(from.Name, to.Name, "", "ProcessWithdrawOrders", ex);
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

        //TODO: Domain
        public static bool IsBaseCurrency(string currency)
        {
            return currency == "ETH" || currency == "BTC";
        }
    }
}