using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitzSharp.Models
{
    public class Ticker
    {
        public long date { get; set; }
        public decimal last { get; set; }
        public decimal buy { get; set; }
        public decimal sell { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal vol { get; set; }
    }
}