using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Config;
using Core.Contracts;
using Core.DbService;
using Core.Models;

namespace Core.Engine
{
    public class EngineHelper
    {

        #region Basic Arbitrage

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
                var tx2 = await counterExchange.Withdraw(transaction.BaseCurrency, baseOrder.CostProceeds + transaction.CounterBaseWithdrawalFee, baseAddress.Address);

                decimal commission = counterOrder.CostProceeds - (baseOrder.CostProceeds + transaction.CounterBaseWithdrawalFee);
                dbService.UpdateTransactionWithdrawalUuid(transaction.Id, tx.Uuid, tx2.Uuid, commission);
            }
            else
            {//Withdraw Market Currency from Counter
                var baseAddress = await baseExchange.GetDepositAddress(transaction.MarketCurrency);
                var tx2 = await counterExchange.Withdraw(transaction.MarketCurrency, counterOrder.Quantity, baseAddress.Address);

                //Withdraw Base Currency from Base
                var counterAddress = await counterExchange.GetDepositAddress(transaction.BaseCurrency);
                var tx = await baseExchange.Withdraw(transaction.BaseCurrency, counterOrder.CostProceeds + transaction.BaseBaseWithdrawalFee, counterAddress.Address);

                decimal commission = baseOrder.CostProceeds - (counterOrder.CostProceeds + transaction.BaseBaseWithdrawalFee);
                dbService.UpdateTransactionWithdrawalUuid(transaction.Id, tx.Uuid, tx2.Uuid, commission);
            }
        }

        public static async Task UpdateTransactionWithdrawalStatus(IDbService dbService, Dictionary<string, IExchange> exchanges, int pId)
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
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (pair.BaseExchangeFee + pair.CounterExchangeFee);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (pair.BaseExchangeFee + pair.CounterExchangeFee);

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

            baseBuy = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.baseBook.asks);
            baseSell = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.baseBook.bids);
            arbBuy = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.asks);
            arbSell = GetPriceAtVolumeThreshold(pair.TradeThreshold, pair.counterBook.bids);

            if (baseBuy > 0 && baseSell > 0 && arbBuy > 0 && arbSell > 0)
            {
                baseBuySpread = Math.Abs((baseBuy - arbSell) / baseBuy) - (pair.BaseExchangeFee + pair.CounterExchangeFee);
                baseSellSpread = Math.Abs((baseSell - arbBuy) / baseSell) - (pair.BaseExchangeFee + pair.CounterExchangeFee);

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

        #endregion

        #region Market Making

        #endregion

        #region Trading Helpers

        public static async Task<IAcceptedAction> Sell(IExchange exchange, ISymbol market, IDbService dbService, string symbol, decimal quantity, decimal price, int txId, string type = "limit")
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "sell", txId, type);
        }

        public static async Task<IAcceptedAction> Buy(IExchange exchange, ISymbol market, IDbService dbService, string symbol, decimal quantity, decimal price, int txId, string type = "limit")
        {
            return await Trade(exchange, market, dbService, symbol, quantity, price, "buy", txId, type);
        }

        public static async Task<IAcceptedAction> Trade(IExchange exchange, ISymbol market, IDbService dbService, string symbol, decimal quantity, decimal price, string side, int txId, string type = "limit")
        {
            IAcceptedAction tradeResult;
            if (side == "buy")
            {
                tradeResult = type == "limit" ? await exchange.LimitBuy(market.ExchangeSymbol, quantity, price) : await exchange.MarketBuy(market.ExchangeSymbol, quantity);
            }
            else
            {
                tradeResult = type == "limit" ? await exchange.LimitSell(market.ExchangeSymbol, quantity, price) : await exchange.MarketSell(market.ExchangeSymbol, quantity);
            }
            return tradeResult;
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

        public static async Task<ArbitragePair> AppendMarketData(IDbService dbService, IExchange baseExchange, IExchange counterExchange, ArbitragePair pair, bool includeBaseMarket = true, bool includeBaseBook = true, bool includeCounterMarket = true, bool includeCounterBook = true)
        {
            if (includeBaseMarket)
                pair.baseMarket = await baseExchange.Ticker(pair.Symbol);

            if(includeBaseBook)
                pair.baseBook = await baseExchange.OrderBook(pair.Symbol);

            if (includeCounterMarket)
                pair.counterMarket = await counterExchange.Ticker(pair.Symbol);

            if (includeCounterBook)
                pair.counterBook = await counterExchange.OrderBook(pair.Symbol);

            if ((pair.baseMarket == null && includeBaseMarket) || (pair.baseBook == null && includeBaseBook) || (pair.counterMarket == null && includeCounterMarket) || (pair.counterBook == null && includeCounterBook))
            {
                var ex = new Exception("No Market Data");
                dbService.LogError(baseExchange.Name, counterExchange.Name, pair.Symbol, "AnalyzeMarket", ex, pair.Id);
                Console.WriteLine("{0}: Null Market Data", pair.Symbol);
                throw ex;
            }

            return pair;
        }

        public static async Task<ArbitragePair> AppendCounterMarketData(IDbService dbService, IExchange counterExchange, ArbitragePair pair, bool includeMarket = true, bool includeBook = true)
        {
            if(includeMarket)
                pair.counterMarket = await counterExchange.Ticker(pair.Symbol);

            if(includeBook)
                pair.counterBook = await counterExchange.OrderBook(pair.Symbol);

            if ((pair.counterMarket == null && includeMarket) || (pair.counterBook == null && includeBook))
            {
                var ex = new Exception("No Market Data");
                dbService.LogError("", counterExchange.Name, pair.Symbol, "AppendCounterMarketData", ex, pair.Id);
                Console.WriteLine("{0}: Null Market Data", pair.Symbol);
                throw ex;
            }

            return pair;
        }

        #endregion

        #region Reporting Helpers

        

        #endregion  


    }
}