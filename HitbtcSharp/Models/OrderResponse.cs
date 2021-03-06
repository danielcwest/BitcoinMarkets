﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
{
    public class HitbtcOrderV1
    {
        public string orderId { get; set; }
        public string orderStatus { get; set; }
        public long lastTimestamp { get; set; }
        public decimal orderPrice { get; set; }
        public decimal orderQuantity { get; set; }
        public decimal avgPrice { get; set; }
        public decimal quantityLeaves { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public string clientOrderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public decimal execQuantity { get; set; }
    }

    public class HitbtcOrderV2 : IAcceptedAction
    {
        /// <summary>
        /// Order ID on the exchange
        /// </summary>
        [JsonProperty("id")]
        public string Uuid { get; set; }
        public string clientOrderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal cumQuantity { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string reportType { get; set; }
        public decimal tradeQuantity { get; set; }
        public decimal tradePrice { get; set; }
        public int tradeId { get; set; }
        public decimal tradeFee { get; set; }
    }

    public class HitbtcOrder : IAcceptedAction
    {
        /// <summary>
        /// Order ID on the exchange
        /// </summary>
        [JsonProperty("id")]
        public string Uuid { get; set; }

        /// <summary>
        /// Order status
        /// new, partiallyFilled, filled, canceled, expired, rejected
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Order quantity, in lots
        /// </summary>
        public decimal? quantity { get; set; }

        /// <summary>
        /// Type of an order
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Time in force
        /// GTC - Good-Til-Canceled, IOC - Immediate-Or-Cancel, OK - Fill-Or-Kill, DAY - day
        /// </summary>
        public string timeInForce { get; set; }

        /// <summary>
        /// Cumulative quantity
        /// </summary>
        public decimal? cumQuantity { get; set; }

        /// <summary>
        /// Unique client-generated ID
        /// </summary>
        public string clientOrderId { get; set; }

        /// <summary>
        /// Currency symbol
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        /// Side of a trade
        /// </summary>
        public string side { get; set; }

        public decimal price { get; set; }

    }

    public class CancelOrderRequest
    {
        public string clientOrderId { get; set; }
        public string cancelRequestClientOrderId { get; set; }
        public string symbol { get; set; } //buy or sell
        public string side { get; set; }
    }

    public class OrderResponse
    {
        public List<HitbtcOrderV1> orders { get; set; }
    }
}
