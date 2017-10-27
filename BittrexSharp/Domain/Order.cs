using System;
using System.Collections.Generic;
using System.Text;
using BMCore.Contracts;

namespace BittrexSharp.Domain
{
    public class Order : IOrder
    {
        public string OrderUuid { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public decimal Cost { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFilled { get; set; }
        public decimal Rate { get; set; }

        public Order(BittrexOrder order)
        {
            this.OrderUuid = order.OrderUuid;
            this.Exchange = order.Exchange;
            this.Symbol = order.Symbol;
            this.Type = order.Type;
            this.Quantity = order.Quantity;
            this.QuantityRemaining = order.QuantityRemaining;
            this.Cost = order.Price;
            this.Rate = order.PricePerUnit.HasValue ? order.PricePerUnit.Value : 0m;
            this.IsOpen = order.IsOpen;
            this.IsFilled = !order.IsOpen && order.QuantityRemaining == 0m;
        }
    }
    public class BittrexOrder
    {
        public string AccountId { get; set; }
        public string OrderUuid { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public decimal Limit { get; set; }
        public decimal Reserved { get; set; }
        public decimal ReservedRemaining { get; set; }
        public decimal CommissionReserved { get; set; }
        public decimal CommissionReservedRemaining { get; set; }
        public decimal CommissionPaid { get; set; }
        public decimal Price { get; set; }
        public decimal? PricePerUnit { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public bool IsOpen { get; set; }
        public string Sentinel { get; set; }
        public bool CancelInitiated { get; set; }
        public bool ImmediateOrCancel { get; set; }
        public bool IsConditional { get; set; }
        public string Condition { get; set; }
        public string ConditionTarget { get; set; }
    }
}
