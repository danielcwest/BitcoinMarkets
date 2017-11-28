using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
{
    public class Transaction
    {
        public string id { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public decimal amount { get; set; }
        public decimal fee { get; set; }
        public decimal networkFee { get; set; }
        public string hash { get; set; }
        public string paymentId { get; set; }
        public string address { get; set; }
    }

    public class TransactionObject
    {
        public Transaction transaction { get; set; }
    }

    public class TransactionList
    {
        public List<Transaction> transactions { get; set; }
    }

    public class PayoutTransaction : IAcceptedAction
    {
        [JsonProperty("transaction")]
        public string Uuid { get; set; }
    }

    public class CryptoTransaction : IAcceptedAction
    {
        [JsonProperty("id")]
        public string Uuid { get; set; }
    }
}
