using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LivecoinSharp.Models
{
    public class Ticker
    {
        public string symbol { get; set; }
        public decimal last { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal volume { get; set; }
        public decimal vwap { get; set; }
        public decimal max_bid { get; set; }
        public decimal min_ask { get; set; }
        public decimal best_bid { get; set; }
        public decimal best_ask { get; set; }
    }
}