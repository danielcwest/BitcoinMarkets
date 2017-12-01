using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface IArbitragePairDTO
    {
        [JsonProperty("id")]
        int Id { get; set; }
        [JsonProperty("tradeThreshold")]
        decimal TradeThreshold { get; set; }
        [JsonProperty("status")]
        string Status { get; set; }
        [JsonProperty("type")]
        string Type { get; set; }
        [JsonProperty("increment")]
        decimal Increment { get; set; }
        [JsonProperty("tickSize")]
        decimal TickSize { get; set; }
        [JsonProperty("askSpread")]
        decimal AskSpread { get; set; }
        [JsonProperty("bidSpread")]
        decimal BidSpread { get; set; }
    }
}
