using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace BittrexSharp.Domain
{
    public class BMMarket : ITicker
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

        //Constructor for Bittrex
        public BMMarket(global::BittrexSharp.Domain.MarketSummary summary)
        {
            this.Exchange = "Bittrex";

            //Bittrex starts the market name with BTC, I prefer end in BTC
            if (summary.MarketName.StartsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = summary.MarketName.Replace("BTC-", "");
                this.MarketName = this.QuoteCurrency + "BTC";
            }
            else if (summary.MarketName.StartsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = summary.MarketName.Replace("ETH-", "");
                this.MarketName = this.QuoteCurrency + "ETH";
            }

            this.Link = string.Format("https://bittrex.com/Market/Index?MarketName={0}", summary.MarketName);

            this.Volume = summary.BaseVolume;
            this.Last = summary.Last;
            this.Timestamp = summary.Timestamp;
            this.Bid = summary.Bid;
            this.Ask = summary.Ask;
        }

    }
}