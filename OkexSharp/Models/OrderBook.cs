using System;
using System.Collections.Generic;
using System.Text;

namespace OkexSharp.Models
{
    public class OrderBookResponse
    {
        public long? timestamp { get; set; }
        public List<List<decimal>> asks { get; set; }
        public List<List<decimal>> bids { get; set; }
    }
}
