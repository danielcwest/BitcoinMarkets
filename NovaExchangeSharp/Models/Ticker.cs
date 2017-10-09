using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NovaExchangeSharp.Models
{
    public class Ticker
    {
        public decimal bid { get; set; }
        public decimal last_price { get; set; }
        public decimal volume24h { get; set; }
        public int marketid { get; set; }
        public int disabled { get; set; }
        public string currency { get; set; }
        public string marketname { get; set; }
        public decimal ask { get; set; }
        public decimal low24h { get; set; }
        public string change24h { get; set; }
        public decimal high24h { get; set; }
        public string basecurrency { get; set; }
    }
}