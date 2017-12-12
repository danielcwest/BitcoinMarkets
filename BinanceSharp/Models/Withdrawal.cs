using Core.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public class Withdrawal : IWithdrawal
    {
        [JsonProperty("id")]
        public string Uuid { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("txId")]
        public string TxId { get; set; }
        [JsonProperty("asset")]
        public string Currency { get; set; }

        public long applyTime { get; set; }
        public int status { get; set; }
    }

    public class WithdrawalResponse : IAcceptedAction
    {
        [JsonProperty("id")]
        public string Uuid { get; set; }
    }
}
