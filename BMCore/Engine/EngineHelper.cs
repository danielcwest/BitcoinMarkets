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
        public static void ExecuteArbitragePairs(Dictionary<string, IExchange> exchanges, BMDbService dbService)
        {
            int pId = -1;

            var groups = dbService.GetArbitragePairs("log").GroupBy(g => new { g.BaseExchange, g.CounterExchange }).Select(g => new ArbitrageGroup()
            {
                BaseExchange = g.Key.BaseExchange,
                CounterExchange = g.Key.CounterExchange,
                Markets = g.Select(m => new ArbitragePair(m))
            });

            foreach (var group in groups)
            {
                var engine = new ArbitrageEngine(exchanges[group.BaseExchange], exchanges[group.CounterExchange], dbService);

                try
                {
                    pId = dbService.StartEngineProcess(group.BaseExchange, group.CounterExchange, "opportunities", new CurrencyConfig());
                    engine.LogOpportunities(group.Markets).Wait();
                    dbService.EndEngineProcess(pId, "success", new { MarketCount = group.Markets.Count() });
                }
                catch (Exception e)
                {
                    dbService.LogError(group.BaseExchange, group.CounterExchange, "", "Main", e, pId);
                    if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
                }

                try
                {
                    pId = dbService.StartEngineProcess(group.BaseExchange, group.CounterExchange, "balances", new CurrencyConfig());
                    engine.CheckBalances(group.Markets).Wait();
                    dbService.EndEngineProcess(pId, "success", new { MarketCount = group.Markets.Count() });
                }
                catch (Exception e)
                {
                    dbService.LogError(group.BaseExchange, group.CounterExchange, "", "Main", e, pId);
                    if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
                }
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
                    if (w != null && !string.IsNullOrWhiteSpace(w.TxId))
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

        public static ArbitrageOpportunity FindOpportunity(ArbitragePair pair)
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;

            baseBuy = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.baseBook.asks);
            baseSell = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.baseBook.bids);
            arbBuy = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.asks);
            arbSell = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (pair.exchangeFees);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (pair.exchangeFees);

                if (baseBuy < arbSell && baseBuySpread >= pair.SpreadThreshold)
                {
                    return new ArbitrageOpportunity()
                    {
                        Type = "basebuy",
                        BasePrice = baseBuy,
                        CounterPrice = arbSell,
                        Spread = baseBuySpread
                    };
                }
                else if (baseSell > arbBuy && baseSellSpread >= pair.SpreadThreshold)
                {
                    return new ArbitrageOpportunity()
                    {
                        Type = "basesell",
                        BasePrice = baseSell,
                        CounterPrice = arbBuy,
                        Spread = baseSellSpread
                    };
                }
            }
            return null;
        }
    }
}