using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMCore.Contracts;

namespace CoinExchangeSharp.Models
{
    public class CoinXMarket
    {
        public int MarketID { get; set; }
        public string MarketAssetName { get; set; }
        public string MarketAssetCode { get; set; }
        public string MarketAssetID { get; set; }
        public string MarketAssetType { get; set; }
        public string BaseCurrency { get; set; }
        public string BaseCurrencyCode { get; set; }
        public string BaseCurrencyID { get; set; }
        public bool Active { get; set; }
    }

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

        //Constructor for CoinExchange
        public Market(CoinExchangeSharp.Models.CoinXMarket market, CoinExchangeSharp.Models.Ticker ticker)
        {
            this.Exchange = "CoinExchange";
            this.BaseCurrency = market.BaseCurrencyCode;
            this.QuoteCurrency = market.MarketAssetCode;
            this.MarketName = this.QuoteCurrency + this.BaseCurrency;
            this.Link = string.Format("https://www.coinexchange.io/market/{0}/{1}", this.QuoteCurrency, this.BaseCurrency);
            this.Volume = ticker.BTCVolume;
            this.Last = ticker.LastPrice;
            this.Timestamp = DateTime.UtcNow;
            this.Bid = ticker.BidPrice;
            this.Ask = ticker.AskPrice;
        }
    }
}