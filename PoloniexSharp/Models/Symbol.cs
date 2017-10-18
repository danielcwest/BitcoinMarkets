using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMCore.Contracts;

namespace PoloniexSharp.Models
{
    public class Symbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public decimal Fee { get; set; }
        public string FeeCurrency { get; set; }

        public Symbol(string name, PoloniexSharp.Models.Ticker ticker)
        {

            ExchangeSymbol = name;

            if (name.StartsWith("BTC"))
            {
                this.BaseCurrency = "BTC";
                this.MarketCurrency = name.Replace("BTC_", "");
                this.LocalSymbol = this.MarketCurrency + "BTC";
                this.FeeCurrency = "BTC";
            }
            else if (name.StartsWith("ETH"))
            {
                this.BaseCurrency = "ETH";
                this.MarketCurrency = name.Replace("ETH_", "");
                this.LocalSymbol = this.MarketCurrency + "ETH";
                this.FeeCurrency = "ETH";
            }

            Fee = 1.0m;
        }
    }
}