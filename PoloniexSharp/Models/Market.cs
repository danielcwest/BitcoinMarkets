using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Models;
using Newtonsoft.Json;

namespace PoloniexSharp.Models
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

        //Constructor for Polo
        public Market(string name, PoloniexSharp.Models.Ticker ticker)
        {
            this.Exchange = "Poloniex";
            //Polo starts the market name with BTC, I prefer end in BTC
            if (name.StartsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = name.Replace("BTC_", "");
                this.MarketName = this.QuoteCurrency + "BTC";
                this.Link = string.Format("https://poloniex.com/exchange#btc_{0}", this.QuoteCurrency);

            }
            else if (name.StartsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = name.Replace("ETH_", "");
                this.MarketName = this.QuoteCurrency + "ETH";
                this.Link = string.Format("https://poloniex.com/exchange#eth_{0}", this.QuoteCurrency);

            }

            this.Volume = decimal.Parse(ticker.baseVolume);
            this.Last = decimal.Parse(ticker.last);
            this.Timestamp = DateTime.UtcNow;
            this.Bid = decimal.Parse(ticker.highestBid);
            this.Ask = decimal.Parse(ticker.lowestAsk);
        }
    }
}

