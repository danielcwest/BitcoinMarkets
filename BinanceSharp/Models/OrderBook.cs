using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BinanceSharp.Models
{
    public class OrderBookResponse
    {
        public long lastUpdateId { get; set; }
        public List<List<object>> bids { get; set; }
        public List<List<object>> asks { get; set; }
    }

    [DataContract]
    public class OrderBookSocketResponse
    {
        [DataMember(Order = 1)]
        [JsonProperty("e")]
        public string eventType { get; set; }
        [DataMember(Order = 2)]
        [JsonProperty("E")]
        public long lastUpdateId { get; set; }
        [DataMember(Order = 3)]
        [JsonProperty("s")]
        public string symbol { get; set; }
        [DataMember(Order = 4)]
        [JsonProperty("u")]
        public long updateId { get; set; }
        [DataMember(Order = 5)]
        [JsonProperty("b")]
        public List<List<object>> bids { get; set; }
        [DataMember(Order = 6)]
        [JsonProperty("a")]
        public List<List<object>> asks { get; set; }
    }
}

