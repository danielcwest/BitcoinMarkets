using Core.Contracts;
using Core.Engine;
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
        public string ClientOrderId { get; set; }
    }
}
