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
        public string Currency { get; set; }
        public decimal Available { get; set; }
        public decimal Held { get; set; }

        public Balance(BalanceResponse res)
        {
            Currency = res.asset;
            Available = res.free;
            Held = res.locked;
        }
    }
}
