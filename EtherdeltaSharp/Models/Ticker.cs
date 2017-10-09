using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtherdeltaSharp.Models
{
    public class Ticker
    {
        public string tokenAddr { get; set; }
        public decimal quoteVolume { get; set; }
        public decimal baseVolume { get; set; }
        public decimal last { get; set; }
        public decimal percentChange { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }
    }
}