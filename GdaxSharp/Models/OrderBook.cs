using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GdaxSharp.Models
{
    public class OrderBookResponse
    {
        public long sequence { get; set; }
        public IEnumerable<string[]> bids { get; set; }
        public IEnumerable<string[]> asks { get; set; }
    }
}

