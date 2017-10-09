using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Models;
using Newtonsoft.Json;

namespace TidexSharp.Models
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

        //Constructor for Tidex
        public Market(string name, TidexSharp.Models.Ticker ticker)
        {
            this.Exchange = "Tidex";
            //Polo starts the market name with BTC, I prefer end in BTC
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

            this.Link = string.Format("https://tidex.com/exchange/{0}/{1}", this.QuoteCurrency, this.BaseCurrency);
            this.Volume = ticker.vol;
            this.Last = ticker.last;
            this.Timestamp = DateTime.UtcNow; //TODO: Parse updated field
            this.Bid = ticker.buy;
            this.Ask = ticker.sell;
        }
    }
}

