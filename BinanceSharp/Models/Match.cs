using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public class Match : IMatch
    {
        public string Uuid { get; set; }
        public string Side { get; set; }
        public string Symbol { get; set; }
        public decimal QuantityFilled { get; set; }
    }
}
