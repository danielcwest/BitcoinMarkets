using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface ITicker
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
