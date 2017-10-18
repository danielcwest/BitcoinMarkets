using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMCore.Contracts;

namespace LiquiSharp.Models
{
    public class Symbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public decimal Fee { get; set; }
        public string FeeCurrency { get; set; }

        public Symbol(string name, LiquiSymbol symbol)
        {
            ExchangeSymbol = name;
            if (name.EndsWith("_btc"))
            {
                this.BaseCurrency = "BTC";
                this.MarketCurrency = name.Replace("_btc", "").ToUpperInvariant();
                this.LocalSymbol = this.MarketCurrency + "BTC";
            }
            else if (name.EndsWith("_eth"))
            {
                this.BaseCurrency = "ETH";
                this.MarketCurrency = name.Replace("_eth", "").ToUpperInvariant();
                this.LocalSymbol = this.MarketCurrency + "ETH";
            }

            Fee = symbol.fee.HasValue ? symbol.fee.Value : 1.0m;
            FeeCurrency = this.BaseCurrency;
        }
    }
}