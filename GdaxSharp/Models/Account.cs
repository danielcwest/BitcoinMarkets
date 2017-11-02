using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class Account
    {
        public Guid id { get; set; }
        public string currency { get; set; }
        public decimal balance { get; set; }
        public decimal hold { get; set; }
        public decimal available { get; set; }
        public bool margin_enabled { get; set; }
        public decimal funded_amount { get; set; }
        public decimal default_amount { get; set; }
    }

    public class CoinbaseAccount
    {
        public string id { get; set; }
        public string name { get; set; }
        public string balance { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public bool primary { get; set; }
        public bool active { get; set; }
    }
}
