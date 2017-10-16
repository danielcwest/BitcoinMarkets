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
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("reserved")]
        public decimal Reserved { get; set; }
    }

    public class MultiCurrencyBalance
    {
        public List<HitBalance> balance { get; set; }
    }
}
