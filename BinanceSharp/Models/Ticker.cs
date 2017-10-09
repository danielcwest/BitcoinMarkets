using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceSharp.Models
{
    public class PriceTicker
    {
        public string symbol { get; set; }
        public decimal price { get; set; }
    }

    public class BookTicker
    {
        public string symbol { get; set; }
        public decimal bidPrice { get; set; }
        public decimal bidQty { get; set; }
        public decimal askPrice { get; set; }
        public decimal askQty { get; set; }
    }

    public class Ticker
    {
        public decimal priceChange { get; set; }
        public decimal priceChangePercent { get; set; }
        public decimal weightedAvgPrice { get; set; }
        public decimal prevClosePrice { get; set; }
        public decimal lastPrice { get; set; }
        public decimal bidPrice { get; set; }
        public decimal askPrice { get; set; }
        public decimal openPrice { get; set; }
        public decimal highPrice { get; set; }
        public decimal lowPrice { get; set; }
        public decimal volume { get; set; }
        public long openTime { get; set; }
        public long closeTime { get; set; }
        public int fristId { get; set; }
        public int lastId { get; set; }
        public int count { get; set; }
    }
}