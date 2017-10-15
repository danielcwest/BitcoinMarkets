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
        public static void ExecuteAllExchanges(IExchange[] exchanges, BMDbService dbService, decimal threshold)
        {
            for (var i = 0; i < exchanges.Length; i++)
            {
                var baseExchange = exchanges[i];
                for (var j = 0; j < exchanges.Length; j++)
                {
                    var arbExchange = exchanges[j];

                    if (baseExchange != arbExchange)
                    {
                        try
                        {
                            Console.WriteLine("Starting: {0} {1}", baseExchange.Name, arbExchange.Name);
                            var engine = new TradingEngine(baseExchange, arbExchange, dbService, threshold);
                            engine.AnalyzeMarkets().Wait();
                            Console.WriteLine("Completed: {0} {1}", baseExchange.Name, arbExchange.Name);

                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e);
                            Console.WriteLine("Error: {0} {1}", baseExchange.Name, arbExchange.Name);
                            dbService.LogError(baseExchange.Name, arbExchange.Name, "", "Main", e.Message, e.StackTrace);

                        }
                        finally
                        {
                            //Thread.Sleep(1000 * 60);
                            Thread.Sleep(100);
                        }
                    }

                }
            }
        }

        public static void ExecuteExchangePair(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, decimal threshold)
        {
            try
            {
                Console.WriteLine("Starting: {0} {1}", baseExchange.Name, arbExchange.Name);
                var engine = new TradingEngine(baseExchange, arbExchange, dbService, threshold);
                engine.AnalyzeMarkets().Wait();
                Console.WriteLine("Completed: {0} {1}", baseExchange.Name, arbExchange.Name);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("Error: {0} {1}", baseExchange.Name, arbExchange.Name);
                dbService.LogError(baseExchange.Name, arbExchange.Name, "", "Main", e.Message, e.StackTrace);

            }
        }

        public static async Task<long> ExecuteTransaction(IExchange baseExchange, IExchange arbExchange, ISymbol market, BMDbService dbService, string symbol, decimal quantity, decimal price, string side)
        {
            //Base Exchange
            long orderId = dbService.InsertOrder(baseExchange.Name, symbol, market.BaseCurrency, market.MarketCurrency, side);

            //Buy On Exchange
            IAcceptedAction tradeResult;
            IDepositAddress depositAddress;

            if (side == "buy")
            {
                depositAddress = await arbExchange.GetDepositAddress(market.MarketCurrency);

                if (string.IsNullOrWhiteSpace(depositAddress.Currency))
                    depositAddress.Currency = market.MarketCurrency; //If we are buying, we need to move the market currency

                tradeResult = await baseExchange.Buy(orderId.ToString(), market.ExchangeSymbol, quantity, price);
            }
            else
            {
                depositAddress = await arbExchange.GetDepositAddress(market.BaseCurrency);

                if (string.IsNullOrWhiteSpace(depositAddress.Currency))
                    depositAddress.Currency = market.BaseCurrency; //If we are executing a sell we will need to move the base currency

                tradeResult = await baseExchange.Sell(orderId.ToString(), market.ExchangeSymbol, quantity, price);
            }

            //Success
            if (!string.IsNullOrWhiteSpace(tradeResult.Uuid))
            {
                var order = await baseExchange.CheckOrder(tradeResult.Uuid);
                string status = order.IsFilled ? "filled" : "open";
                dbService.UpdateOrderUuid(orderId, order.OrderUuid, status, 0, order.Quantity, order.Price);

                //TODO: This should be done before executing trades
                if (string.IsNullOrWhiteSpace(depositAddress.Address))
                    throw new Exception(string.Format("No deposit Address for {0} at {1} exchange", depositAddress.Currency, arbExchange.Name));

                var withdrawalResult = await baseExchange.Withdraw(market.MarketCurrency, order.Quantity, depositAddress.Address);

                return dbService.InsertWithdrawal(withdrawalResult.Uuid, orderId, depositAddress.Currency, baseExchange.Name, order.Quantity);
            }

            return await Task.FromResult(0);
        }

        public static async Task UpdateWithdrawals(IDbService dbService, Dictionary<string, IExchange> exchanges)
        {
            foreach (var withdrawal in dbService.GetWithdrawals().Where(w => string.IsNullOrEmpty(w.TxId)))
            {
                var exchange = exchanges[withdrawal.FromExchange];
                var w = await exchange.GetWithdrawal(withdrawal.Uuid);
                if (w != null && !string.IsNullOrWhiteSpace(w.TxId))
                    dbService.UpdateWithdrawal(withdrawal.Id, withdrawal.CounterId, w.Amount, w.TxId);
            }
            await Task.FromResult(0);
        }

    }
}