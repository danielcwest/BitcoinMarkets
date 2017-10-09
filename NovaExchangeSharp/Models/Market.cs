using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Models;
using Newtonsoft.Json;

namespace NovaExchangeSharp.Models
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

        //Constructor for Nova
        public Market(NovaExchangeSharp.Models.Ticker ticker)
        {
            this.Exchange = "NovaExchange";
            this.BaseCurrency = ticker.basecurrency;
            this.QuoteCurrency = ticker.currency;
            this.MarketName = this.QuoteCurrency + this.BaseCurrency;
            this.Link = string.Format("https://novaexchange.com/market/{0}/", ticker.marketname);
            this.Volume = ticker.volume24h;
            this.Last = ticker.last_price;
            this.Timestamp = DateTime.UtcNow;
            this.Bid = ticker.bid;
            this.Ask = ticker.ask;
        }
    }
}