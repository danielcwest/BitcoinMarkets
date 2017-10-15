using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace LivecoinSharp.Models
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

        //Constructor for Livecoin
        public Market(LivecoinSharp.Models.Ticker summary)
        {
            this.Exchange = "Livecoin";

            //BTC Market
            if (summary.symbol.EndsWith("/BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = summary.symbol.Replace("/BTC", "");
                this.MarketName = this.QuoteCurrency + "BTC";
            }//Eth Market
            else if (summary.symbol.EndsWith("/ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = summary.symbol.Replace("/ETH", "");
                this.MarketName = this.QuoteCurrency + "ETH";
            }

            this.Link = string.Format("https://www.livecoin.net/en/trade/index?currencyPair={0}", summary.symbol);
            this.Volume = summary.volume * summary.vwap;
            this.Last = summary.last;
            this.Timestamp = DateTime.UtcNow;
            this.Bid = summary.max_bid;
            this.Ask = summary.min_ask;
        }
    }
}