using System;
using System.Collections.Generic;
using System.Text;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace BittrexSharp.Domain
{
    public class HistoricWithdrawal : IWithdrawal
    {
        [JsonProperty("PaymentUuid")]
        public string Uuid { get; set; }
        [JsonProperty("Currency")]
        public string Currency { get; set; }
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
        [JsonProperty("Address")]
        public string Address { get; set; }
        public DateTime Opened { get; set; }
        public bool Authorized { get; set; }
        [JsonProperty("PendingPayment")]
        public bool Pending { get; set; }
        [JsonProperty("TxCost")]
        public decimal TxCost { get; set; }
        [JsonProperty("TxId")]
        public string TxId { get; set; }
        public bool Canceled { get; set; }
        public bool InvalidAddress { get; set; }
    }
}
