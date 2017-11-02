using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace BinanceSharp.Models
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

        //Binance
        public Market(BinanceSharp.Models.PriceTicker price, BinanceSharp.Models.BookTicker book)
        {
            this.Exchange = "Binance";
            this.MarketName = price.symbol;

            if (price.symbol.EndsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = price.symbol.Replace("BTC", "");
                this.Link = string.Format("https://www.binance.com/trade.html?symbol={0}_BTC", this.QuoteCurrency);

            }
            else if (price.symbol.EndsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = price.symbol.Replace("ETH", "");
                this.Link = string.Format("https://www.binance.com/trade.html?symbol={0}_ETH", this.QuoteCurrency);
            }

            this.Volume = book.askQty + book.bidQty;
            this.Last = price.price;
            this.Timestamp = DateTime.UtcNow; //TODO: Parse updated field
            this.Bid = book.bidPrice;
            this.Ask = book.askPrice;
        }

        public Market(string symbol, BinanceSharp.Models.Ticker ticker)
        {
            this.Exchange = "Binance";
            this.MarketName = symbol;

            if (symbol.EndsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.QuoteCurrency = symbol.Replace("BTC", "");
                this.Link = string.Format("https://www.binance.com/trade.html?symbol={0}_BTC", this.QuoteCurrency);

            }
            else if (symbol.EndsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.QuoteCurrency = symbol.Replace("ETH", "");
                this.Link = string.Format("https://www.binance.com/trade.html?symbol={0}_ETH", this.QuoteCurrency);
            }

            this.Volume = ticker.volume;
            this.Last = ticker.lastPrice;
            this.Timestamp = DateTime.UtcNow; //TODO: Parse updated field
            this.Bid = ticker.bidPrice;
            this.Ask = ticker.askPrice;
        }
    }
}