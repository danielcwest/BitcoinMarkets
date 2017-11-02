using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GdaxSharp.Models
{
    public class Ticker
    {
        public string trade_id { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }
        public decimal volume { get; set; }
        public DateTime time { get; set; }
    }
}