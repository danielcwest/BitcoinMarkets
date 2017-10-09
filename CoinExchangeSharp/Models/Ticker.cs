using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoinExchangeSharp.Models
{
    public class Ticker
    {
        public int MarketID { get; set; }
        public decimal LastPrice { get; set; }
        public string Change { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal Volume { get; set; }
        public decimal BTCVolume { get; set; }
        public int TradeCount { get; set; }
        public decimal BidPrice { get; set; }
        public decimal AskPrice { get; set; }
        public int BuyOrderCount { get; set; }
        public int SellOrderCount { get; set; }
    }
}