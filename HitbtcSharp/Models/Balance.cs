using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
{
    public class HitBalance : ICurrencyBalance
    {
        /// <summary>
        /// Currency symbol, e.g. BTC
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Funds amount
        /// </summary>
        [JsonProperty("available")]
        public decimal Available { get; set; }
        [JsonProperty("reserved")]
        public decimal Held { get; set; }
    }
}
