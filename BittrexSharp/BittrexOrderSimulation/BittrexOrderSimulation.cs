﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BittrexSharp.Domain;
using System.Linq;

namespace BittrexSharp.BittrexOrderSimulation
{
    /// <summary>
    /// Behaves exactly like Bittrex, except that buy and sell orders are not put through to Bittrex, and are simulated instead
    /// </summary>
    public class BittrexOrderSimulation : BittrexApi
    {
        private List<OpenOrder> simulatedOpenOrders = new List<OpenOrder>();
        private List<BittrexOrder> simulatedFinishedOrders = new List<BittrexOrder>();
        private List<BittrexBalance> simulatedBalances = new List<BittrexBalance>();

        public BittrexOrderSimulation(string apiKey, string apiSecret) : base(apiKey, apiSecret)
        {
        }

        private void addBalance(string currency, decimal quantity)
        {
            var existingBalance = simulatedBalances.SingleOrDefault(b => b.Currency == currency);
            if (existingBalance != null)
                existingBalance.Available += quantity;
            else
                simulatedBalances.Add(new BittrexBalance
                {
                    Available = quantity,
                    Currency = currency
                });
        }

        private void removeBalance(string currency, decimal quantity)
        {
            var existingBalance = simulatedBalances.Single(b => b.Currency == currency);
            existingBalance.Available -= quantity;
        }

        public override async Task<AcceptedOrder> BuyLimit(string marketName, decimal quantity, decimal rate)
        {
            var currentRate = (await GetTicker(marketName)).Last;

            var acceptedOrderId = Guid.NewGuid().ToString();
            if (currentRate <= rate)
            {
                var order = new BittrexOrder
                {
                    Closed = DateTime.Now,
                    Exchange = marketName,
                    IsOpen = false,
                    Limit = rate,
                    Opened = DateTime.Now,
                    OrderUuid = acceptedOrderId,
                    Price = quantity * rate,
                    PricePerUnit = rate,
                    Quantity = quantity
                };
                simulatedFinishedOrders.Add(order);

                var currency = Helper.GetTargetCurrencyFromMarketName(marketName);
                addBalance(currency, quantity);
            }
            else
            {
                var order = new OpenOrder
                {
                    Closed = DateTime.Now,
                    Exchange = marketName,
                    Limit = rate,
                    Opened = DateTime.Now,
                    OrderUuid = acceptedOrderId,
                    Price = quantity * rate,
                    PricePerUnit = rate,
                    Quantity = quantity
                };
                simulatedOpenOrders.Add(order);
            }

            return new AcceptedOrder
            {
                Uuid = acceptedOrderId
            };
        }

        public override async Task<AcceptedOrder> SellLimit(string marketName, decimal quantity, decimal rate)
        {
            var currentRate = (await GetTicker(marketName)).Last;

            var acceptedOrderId = Guid.NewGuid().ToString();
            if (currentRate >= rate)
            {
                var order = new BittrexOrder
                {
                    Closed = DateTime.Now,
                    Exchange = marketName,
                    IsOpen = false,
                    Limit = rate,
                    Opened = DateTime.Now,
                    OrderUuid = acceptedOrderId,
                    Price = -quantity * rate,
                    PricePerUnit = rate,
                    Quantity = -quantity
                };
                simulatedFinishedOrders.Add(order);

                var currency = Helper.GetTargetCurrencyFromMarketName(marketName);
                removeBalance(currency, quantity);
            }
            else
            {
                var order = new OpenOrder
                {
                    Closed = DateTime.Now,
                    Exchange = marketName,
                    Limit = rate,
                    Opened = DateTime.Now,
                    OrderUuid = acceptedOrderId,
                    Price = -quantity * rate,
                    PricePerUnit = rate,
                    Quantity = -quantity
                };
                simulatedOpenOrders.Add(order);
            }

            return new AcceptedOrder
            {
                Uuid = acceptedOrderId
            };
        }

        public override async Task CancelOrder(string orderId)
        {
            var order = simulatedOpenOrders.Single(o => o.OrderUuid == orderId);
            simulatedOpenOrders.Remove(order);
            await Task.FromResult(0);
        }

        public override async Task<IEnumerable<OpenOrder>> GetOpenOrders(string marketName = null)
        {
            if (marketName == null) return simulatedOpenOrders;
            else return await Task.FromResult(simulatedOpenOrders.Where(o => o.Exchange == marketName).ToList());

        }

        public override async Task<IEnumerable<BittrexBalance>> GetBalances()
        {
            return await Task.FromResult(simulatedBalances);
        }

        public override async Task<BittrexBalance> GetBalance(string currency)
        {
            return await Task.FromResult(simulatedBalances.SingleOrDefault(b => b.Currency == currency) ?? new BittrexBalance
            {
                Available = 0,
                Currency = currency
            });
        }

        public override async Task<BittrexOrder> GetOrder(string orderId)
        {
            var openOrder = simulatedOpenOrders.SingleOrDefault(o => o.OrderUuid == orderId);
            if (openOrder == null) return simulatedFinishedOrders.SingleOrDefault(o => o.OrderUuid == orderId);

            return await Task.FromResult(new BittrexOrder
            {
                Closed = openOrder.Closed,
                Exchange = openOrder.Exchange,
                Limit = openOrder.Limit,
                Opened = openOrder.Opened,
                OrderUuid = openOrder.OrderUuid,
                Price = openOrder.Price,
                PricePerUnit = openOrder.PricePerUnit,
                Quantity = openOrder.Quantity
            });
        }

        public override async Task<IEnumerable<HistoricOrder>> GetOrderHistory(string marketName = null)
        {
            return await Task.FromResult(simulatedFinishedOrders.Where(o => o.Exchange == marketName).Select(o => new HistoricOrder
            {
                Exchange = o.Exchange,
                Limit = o.Limit,
                OrderUuid = o.OrderUuid,
                Price = o.Price,
                PricePerUnit = o.PricePerUnit,
                Quantity = o.Quantity,
                Timestamp = o.Closed.Value
            }).ToList());
        }
    }
}
