using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Models;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
{
    public class Order : IOrder
    {
        public string OrderUuid { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public decimal Price { get; set; }
        public bool IsOpen { get; set; }
        public string Side { get; set; }
        public string ClientOrderId { get; set; }

        public Order(HitbtcSharp.Models.HitbtcOrder order)
        {
            this.OrderUuid = order.orderId.ToString();
            this.Exchange = "Hitbtc";
            this.Symbol = order.symbol;
            this.Type = order.type;
            this.Quantity = Convert.ToDecimal(order.orderQuantity);
            this.QuantityRemaining = Convert.ToDecimal(order.quantityLeaves);
            this.Price = Convert.ToDecimal(order.orderPrice);
            this.Side = order.side;
            this.ClientOrderId = order.clientOrderId;
        }
    }
}