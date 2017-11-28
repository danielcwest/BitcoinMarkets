using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using Core.Contracts;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
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

        public Market(string name, HitbtcSharp.Models.Ticker ticker)
        {
            this.Exchange = "Hitbtc";
            this.MarketName = name;
            this.Volume = ticker.volumeQuote.HasValue ? ticker.volumeQuote.Value : 0;
            this.Last = ticker.last.HasValue ? ticker.last.Value : 0;
            this.Timestamp = ticker.timestamp;
            this.Bid = ticker.bid.HasValue ? ticker.bid.Value : 0;
            this.Ask = ticker.ask.HasValue ? ticker.ask.Value : 0;

            if (this.MarketName.EndsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = this.MarketName.Replace("BTC", "");
                this.Link = string.Format("https://hitbtc.com/exchange/{0}-to-BTC", this.QuoteCurrency);

            }
            else if (this.MarketName.EndsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = this.MarketName.Replace("ETH", "");
                this.Link = string.Format("https://hitbtc.com/exchange/{0}-to-ETH", this.QuoteCurrency);
            }
        }
    }
}