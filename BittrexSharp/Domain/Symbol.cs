using System;
using System.Collections.Generic;
using System.Text;
using BMCore.Contracts;

namespace BittrexSharp.Domain
{
    public class Symbol : ISymbol
    {
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public decimal Fee { get; set; }
        public string LocalSymbol { get; set; }
        public string FeeCurrency { get; set; }
        public Symbol(Market market)
        {
            ExchangeSymbol = market.MarketName;
            MarketCurrency = market.MarketCurrency;
            BaseCurrency = market.BaseCurrency;
            Fee = 1.0m;
            LocalSymbol = string.Format("{0}{1}", MarketCurrency, BaseCurrency);
        }
    }
}