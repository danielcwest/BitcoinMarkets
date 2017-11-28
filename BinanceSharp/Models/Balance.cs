using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;
using Newtonsoft.Json;

namespace BinanceSharp.Models
{
    public class Balance : ICurrencyBalance
    {
        [JsonProperty("asset")]
        public string Currency { get; set; }
        [JsonProperty("free")]
        public decimal Available { get; set; }
        [JsonProperty("locked")]
        public decimal Held { get; set; }
    }
}
