using Core.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class WithdrawalResponse : IAcceptedAction
    {
        [JsonProperty("id")]
        public string Uuid { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
    }
}
