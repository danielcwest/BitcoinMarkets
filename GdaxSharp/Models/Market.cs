using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace GdaxSharp.Models
{
    public class Market : ITicker
    {
        public string Exchange { get; set; }
        public string MarketName { get; set; }
        public decimal Volume { get; set; }
        public decimal Last { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public string QuoteCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string Link { get; set; }

        public Market(string symbol, Ticker ticker)
        {
            Exchange = "Gdax";
            MarketName = symbol;
            Volume = ticker.volume;
            Last = ticker.price;
            Timestamp = ticker.time;
            Bid = ticker.bid;
            Ask = ticker.ask;

            if (this.MarketName.EndsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = this.MarketName.Replace("BTC", "");
            }
            else if (this.MarketName.EndsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = this.MarketName.Replace("ETH", "");
            }
        }
    }
}