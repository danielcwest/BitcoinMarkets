using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace BinanceSharp.Models
{
    /// <summary>
    /// Shared class that represents either a trade or an order, and the data returned from the WebSocket endpoint
    /// </summary>
    [DataContract]
    public class UserDataResponse : IWebSocketResponse
    {
        [DataMember(Order = 1)]
        [JsonProperty(PropertyName = "e")]
        public string EventType { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(PropertyName = "E")]
        public long EventTime { get; set; }

        [DataMember(Order = 3)]
        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [DataMember(Order = 4)]
        [JsonProperty(PropertyName = "c")]
        public string NewClientOrderId { get; set; }

        [DataMember(Order = 5)]
        [JsonProperty(PropertyName = "S")]
        public string Side { get; set; }

        [DataMember(Order = 6)]
        [JsonProperty(PropertyName = "o")]
        public string Type { get; set; }

        [DataMember(Order = 7)]
        [JsonProperty(PropertyName = "f")]
        public string TimeInForce { get; set; }

        [DataMember(Order = 7)]
        [JsonProperty(PropertyName = "q")]
        public decimal Quantity { get; set; }

        [DataMember(Order = 8)]
        [JsonProperty(PropertyName = "p")]
        public double Price { get; set; }

        #region Undefined API Result fields
        //TODO: Update when Binance API updated
        [DataMember(Order = 9)]
        [JsonProperty(PropertyName = "P")]
        public double P { get; set; }

        [DataMember(Order = 10)]
        [JsonProperty(PropertyName = "F")]
        public double F { get; set; }

        [DataMember(Order = 11)]
        [JsonProperty(PropertyName = "g")]
        public string G { get; set; }

        [DataMember(Order = 12)]
        [JsonProperty(PropertyName = "C")]
        public string C { get; set; }
        #endregion

        [DataMember(Order = 13)]
        [JsonProperty(PropertyName = "x")]
        public string ExecutionType { get; set; }

        [DataMember(Order = 14)]
        [JsonProperty(PropertyName = "X")]
        public string OrderStatus { get; set; }

        [DataMember(Order = 15)]
        [JsonProperty(PropertyName = "r")]
        public string OrderRejectReason { get; set; }

        #region Undefined API Result fields
        //TODO: Update when Binance API updated
        [DataMember(Order = 16)]
        [JsonProperty(PropertyName = "i")]
        public long OrderId { get; set; }

        [DataMember(Order = 17)]
        [JsonProperty(PropertyName = "l")]
        public decimal QuantityOfLastFilledTrade { get; set; }

        [DataMember(Order = 18)]
        [JsonProperty(PropertyName = "z")]
        public decimal AccumulatedQuantityOfFilledTradesThisOrder { get; set; }

        [DataMember(Order = 19)]
        [JsonProperty(PropertyName = "L")]
        public decimal PriceOfLastFilledTrade { get; set; }

        [DataMember(Order = 20)]
        [JsonProperty(PropertyName = "n")]
        public decimal Commission { get; set; }

        /// <summary>
        /// Asset on which commission taken
        /// </summary>
        [DataMember(Order = 21)]
        [JsonProperty(PropertyName = "N")]
        public string AssetCommissionTakenFrom { get; set; }
        #endregion

        /// <summary>
        /// Represents Order or Trade time
        /// </summary>
        [DataMember(Order = 22)]
        [JsonProperty(PropertyName = "T")]
        public long TimeStamp { get; set; }

        [DataMember(Order = 23)]
        [JsonProperty(PropertyName = "t")]
        public long TradeId { get; set; }

        #region Undefined API Result fields
        //TODO: Update when Binance API updated
        //[DataMember(Order = 25)]
        //[JsonProperty(PropertyName = "l")]
        //public long l { get; set; }

        //[DataMember(Order = 26)]
        //[JsonProperty(PropertyName = "w")]
        //public bool w { get; set; }
        #endregion

        //[DataMember(Order = 27)]
        //[JsonProperty(PropertyName = "m")]
        //public bool IsBuyerMaker { get; set; }

        #region Undefined API Result fields
        //TODO: Update when Binance API updated
        //[DataMember(Order = 28)]
        //[JsonProperty(PropertyName = "M")]
        //public bool M { get; set; }
        #endregion
    }
}
