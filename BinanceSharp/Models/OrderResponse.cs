using Core.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public class OrderResponse : IAcceptedAction
    {
        [JsonProperty("orderId")]
        public string Uuid { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("transactTime")]
        public long TransactTime { get; set; }
    }
}
