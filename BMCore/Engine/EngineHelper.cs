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
        public static void LogOpportunities(Dictionary<string, IExchange> exchanges, BMDbService dbService)
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
                    engine.LogOpportunities(group.Markets, pId).Wait();
                    dbService.EndEngineProcess(pId, "success", new { MarketCount = group.Markets.Count() });
                }
                catch (Exception e)
                {
                    dbService.LogError(group.BaseExchange, group.CounterExchange, "", "Main", e, pId);
                    if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
                }
            }
        }

        public static async Task ExecuteTradePairs(Dictionary<string, IExchange> exchanges, BMDbService dbService)
        {
            int pId = -1;

            var groups = dbService.GetArbitragePairs("trade").GroupBy(g => new { g.BaseExchange, g.CounterExchange }).Select(g => new ArbitrageGroup()
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
                    pId = dbService.StartEngineProcess(group.BaseExchange, group.CounterExchange, "trade", new CurrencyConfig());
                    await engine.ExecuteTrades(group.Markets, pId);
                    dbService.EndEngineProcess(pId, "success", new { MarketCount = group.Markets.Count() });

                }
                catch (Exception e)
                {
                    dbService.LogError(group.BaseExchange, group.CounterExchange, "", "Main", e, pId);
                    if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
                }
            }
        }

        public static void CheckExchangeBalances(Dictionary<string, IExchange> exchanges, BMDbService dbService)
        {
            int pId = -1;

            var groups = dbService.GetArbitragePairs("").GroupBy(g => new { g.BaseExchange, g.CounterExchange }).Select(g => new ArbitrageGroup()
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
                    pId = dbService.StartEngineProcess(group.BaseExchange, group.CounterExchange, "balances", new CurrencyConfig());
                    engine.CheckBalances(group.Markets, pId).Wait();
                    dbService.EndEngineProcess(pId, "success", new { MarketCount = group.Markets.Count() });
                }
                catch (Exception e)
                {
                    dbService.LogError(group.BaseExchange, group.CounterExchange, "", "Main", e, pId);
                    if (pId > 0) dbService.EndEngineProcess(pId, "error", e);
                }
            }
        }

        public static async Task<IAcceptedAction> Sell(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, int txId)
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "sell", txId);
        }

        public static async Task<IAcceptedAction> Buy(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, int txId)
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "buy", txId);
        }

        public static async Task<IAcceptedAction> Trade(IExchange exchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, string side, int txId)
        {
            IAcceptedAction tradeResult;
            if (side == "buy")
            {
                tradeResult = await exchange.Buy(txId.ToString().PadLeft(8, '0'), market.ExchangeSymbol, quantity, price);
            }
            else
            {
                tradeResult = await exchange.Sell(txId.ToString().PadLeft(8, '0'), market.ExchangeSymbol, quantity, price);
            }
            return tradeResult;
        }

        public static async Task ProcessTransactionOrders(IDbService dbService, Dictionary<string, IExchange> exchanges)
        {
            int pId = dbService.StartEngineProcess("All Exchanges", "AllExchanges", "withdraw", new CurrencyConfig());

            foreach (var transaction in dbService.GetTransactions(status: "orderpending"))
            {
                try
                {
                    var baseExchange = exchanges[transaction.BaseExchange];
                    var counterExchange = exchanges[transaction.CounterExchange];

                    var baseOrder = await baseExchange.CheckOrder(transaction.BaseOrderUuid);
                    var counterOrder = await counterExchange.CheckOrder(transaction.CounterOrderUuid);

                    if (baseOrder != null && baseOrder.IsFilled && counterOrder != null && counterOrder.IsFilled)
                    {
                        dbService.UpdateTransactionStatus(transaction.Id, "ordercomplete", new { bOrder = baseOrder, cOrder = counterOrder });
                        await WithdrawTransactions(dbService, exchanges, transaction, baseOrder, counterOrder, pId);
                    }
                }
                catch (Exception ex)
                {
                    //dbService.UpdateWithdrawalStatus(withdrawal.Id, "error", ex);
                    dbService.LogError(transaction.BaseExchange, transaction.CounterExchange, transaction.Id.ToString(), "ProcessTransactionOrders", ex, pId);
                }
            }
            dbService.EndEngineProcess(pId, "success");
        }

        private static async Task WithdrawTransactions(IDbService dbService, Dictionary<string, IExchange> exchanges, DbTransaction transaction, IOrder baseOrder, IOrder counterOrder, int pId)
        {
            IExchange baseExchange = exchanges[transaction.BaseExchange];
            IExchange counterExchange = exchanges[transaction.CounterExchange];

            if (transaction.Type == "basebuy")
            {//Withdraw Market Currency from Base
                var counterAddress = await counterExchange.GetDepositAddress(transaction.MarketCurrency);
                var tx = await baseExchange.Withdraw(transaction.MarketCurrency, baseOrder.Quantity, counterAddress.Address);

                //Withdraw Base Currency from Counter
                var baseAddress = await baseExchange.GetDepositAddress(transaction.BaseCurrency);
                var tx2 = await counterExchange.Withdraw(transaction.BaseCurrency, transaction.TradeThreshold, baseAddress.Address);
                dbService.UpdateTransactionWithdrawalUuid(transaction.Id, tx.Uuid, tx2.Uuid);
            }
            else
            {//Withdraw Base Currency from Base
                var counterAddress = await counterExchange.GetDepositAddress(transaction.BaseCurrency);
                var tx = await baseExchange.Withdraw(transaction.BaseCurrency, transaction.TradeThreshold, counterAddress.Address);

                //Withdraw Market Currency from Counter
                //We want to transfer the amount of Market Currency we SOLD on the base exchange, 
                //something like baseOrder.Quantity + withdrwawal fee
                var baseAddress = await baseExchange.GetDepositAddress(transaction.MarketCurrency);
                var tx2 = await counterExchange.Withdraw(transaction.MarketCurrency, counterOrder.Quantity, baseAddress.Address);
                dbService.UpdateTransactionWithdrawalUuid(transaction.Id, tx.Uuid, tx2.Uuid);
            }

            Thread.Sleep(1000 * 60);

            await UpdateTransactionWithdrawalStatus(dbService, exchanges, pId);
        }

        private static async Task UpdateTransactionWithdrawalStatus(IDbService dbService, Dictionary<string, IExchange> exchanges, int pId)
        {
            foreach (var transaction in dbService.GetTransactions(status: "withdrawalpending"))
            {
                try
                {
                    var baseExchange = exchanges[transaction.BaseExchange];
                    var counterExchange = exchanges[transaction.CounterExchange];

                    var baseW = await baseExchange.GetWithdrawal(transaction.BaseWithdrawalUuid);
                    var counterW = await counterExchange.GetWithdrawal(transaction.CounterWithdrawalUuid);

                    if (baseW != null && !string.IsNullOrWhiteSpace(baseW.TxId) && counterW != null && !string.IsNullOrWhiteSpace(counterW.TxId))
                        dbService.CloseTransaction(transaction.Id, baseW.TxId, counterW.TxId);
                }
                catch (Exception ex)
                {
                    //dbService.UpdateWithdrawalStatus(withdrawal.Id, "error", ex);
                    dbService.LogError(transaction.BaseExchange, transaction.CounterExchange, transaction.Id.ToString(), "UpdateTransactionWithdrawalStatus", ex, pId);
                }
            }
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

        public static ArbitrageOpportunity FakeOpportunity(ArbitragePair pair)
        {
            decimal baseBuy = 0M;
            decimal baseSell = 0M;
            decimal arbBuy = 0M;
            decimal arbSell = 0M;
            decimal baseBuySpread = 0M;
            decimal baseSellSpread = 0M;

            pair.TradeThreshold = 0.25m;

            baseBuy = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.baseBook.asks);
            baseSell = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.baseBook.bids);
            arbBuy = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.asks);
            arbSell = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (pair.exchangeFees);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (pair.exchangeFees);

                return new ArbitrageOpportunity()
                {
                    Type = "basesell",
                    BasePrice = baseSell,
                    CounterPrice = arbBuy,
                    Spread = baseSellSpread
                };
            }
            return null;
        }
    }
}