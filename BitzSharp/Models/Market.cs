using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Models;
using Newtonsoft.Json;

namespace BitzSharp.Models
{
    public class Market : IMarket
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

        //Constructor for Bit-z
        public Market(string name, BitzSharp.Models.Ticker ticker)
        {
            this.Exchange = "Bit-z";
            if (name.EndsWith("_btc"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = name.Replace("_btc", "").ToUpperInvariant();
                this.MarketName = this.QuoteCurrency + "BTC";
            }
            else if (name.EndsWith("_eth"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = name.Replace("_eth", "").ToUpperInvariant();
                this.MarketName = this.QuoteCurrency + "ETH";
            }

            this.Link = string.Format("https://www.bit-z.com/trade/{0}", name);
            this.Volume = (ticker.vol.HasValue && ticker.last.HasValue) ? ticker.vol.Value * ticker.last.Value : 0M;
            this.Last = ticker.last.HasValue ? ticker.last.Value : 0M;
            this.Timestamp = Utils.UnixTimeStampToDateTimeUtc(ticker.date);
            this.Bid = ticker.buy;
            this.Ask = ticker.sell;
        }
    }
}