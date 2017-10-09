using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoloniexSharp.Models
{
    public class Ticker
    {
        public int id { get; set; }
        public string last { get; set; }
        public string lowestAsk { get; set; }
        public string highestBid { get; set; }
        public string percentChange { get; set; }
        public string baseVolume { get; set; }
        public string quoteVolume { get; set; }
        public string isFrozen { get; set; }
        public string high24hr { get; set; }
        public string low24hr { get; set; }
    }
}