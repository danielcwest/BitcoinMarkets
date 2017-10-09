using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceSharp.Models
{
    public class OrderBookResponse
    {
        public long lastUpdateId { get; set; }
        public List<List<object>> bids { get; set; }
        public List<List<object>> asks { get; set; }
    }
}

