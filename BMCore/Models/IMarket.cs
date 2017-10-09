using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using Newtonsoft.Json;

namespace BMCore.Models
{
    public interface IMarket
    {
        [JsonProperty("exchange")]
        string Exchange { get; set; }

        [JsonProperty("symbol")]
        string MarketName { get; set; }

        [JsonProperty("volume")]
        decimal Volume { get; set; }

        [JsonProperty("last")]
        decimal Last { get; set; }

        [JsonProperty("timestamp")]
        DateTime Timestamp { get; set; }

        [JsonProperty("bid")]
        decimal Bid { get; set; }

        [JsonProperty("ask")]
        decimal Ask { get; set; }

        [JsonProperty("quoteCurrency")]
        string QuoteCurrency { get; set; }

        [JsonProperty("baseCurrency")]
        string BaseCurrency { get; set; }

        [JsonProperty("link")]
        string Link { get; set; }
    }
}