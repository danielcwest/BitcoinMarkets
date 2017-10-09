using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Models;
using Newtonsoft.Json;

namespace EtherdeltaSharp.Models
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

        //Constructor for Etherdelta
        public Market(string name, EtherdeltaSharp.Models.Ticker ticker)
        {
            this.Exchange = "Etherdelta";
            this.QuoteCurrency = name.Replace("ETH_", "");
            this.MarketName = this.QuoteCurrency + "ETH";
            this.BaseCurrency = "ETH";
            this.Link = string.Format("https://etherdelta.com/#{0}-ETH", this.QuoteCurrency);
            this.Volume = ticker.baseVolume;
            this.Last = ticker.last;
            this.Timestamp = DateTime.UtcNow;
            this.Bid = ticker.bid;
            this.Ask = ticker.ask;
        }
    }
}