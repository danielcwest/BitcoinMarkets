using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class Fill
    {
        public int trade_id { get; set; }
        public string product_id { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public string order_id { get; set; }
        public DateTime created_at { get; set; }
        public string liquidity { get; set; }
        public decimal fee { get; set; }
        public bool settled { get; set; }
        public string side { get; set; }
    }
}
