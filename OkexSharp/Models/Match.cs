using Core.Contracts;
using Core.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkexSharp.Models
{
    public class Match : IMatch
    {
        public string Uuid { get; set; }
        public OrderSide Side { get; set; }
        public string Symbol { get; set; }
        public decimal QuantityFilled { get; set; }
        public string ClientOrderId { get; set; }
    }
}
