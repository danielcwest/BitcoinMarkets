using System;
using System.Collections.Generic;
using System.Text;

namespace BMCore.Models
{
    public class OrderBookEntry
    {
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal sum { get; set; }
        public decimal sumBase { get; set; }

    }
}
