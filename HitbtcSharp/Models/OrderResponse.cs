using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
{
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
        public string orderStatus { get; set; }

        /// <summary>
        /// UTC timestamp of the last change, in milliseconds
        /// </summary>
        public long lastTimestamp { get; set; }

        /// <summary>
        /// Order price
        /// </summary>
        public string orderPrice { get; set; }

        /// <summary>
        /// Order quantity, in lots
        /// </summary>
        public int quantity { get; set; }

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
        public int? cumQuantity { get; set; }

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
        public List<HitbtcOrder> orders { get; set; }
    }
}
