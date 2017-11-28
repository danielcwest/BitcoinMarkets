using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public class BinanceTrade
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal commission { get; set; }
        public string commissionAsset { get; set; }
        public long time { get; set; }
        public bool isBuyer { get; set; }
        public bool isMaker { get; set; }
        public bool isBestMatch { get; set; }
    }
}
