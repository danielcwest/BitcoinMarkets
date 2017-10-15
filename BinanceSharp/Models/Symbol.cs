using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMCore.Contracts;

namespace BinanceSharp.Models
{
    public class Symbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public decimal Fee { get; set; }

        public Symbol(PriceTicker ticker)
        {
            LocalSymbol = ticker.symbol;
            ExchangeSymbol = ticker.symbol;
            Fee = 1.0m;

            if (ticker.symbol.EndsWith("BTC"))
            {
                BaseCurrency = "BTC";
                MarketCurrency = ticker.symbol.Replace("BTC", "");
            }
            else if (ticker.symbol.EndsWith("ETH"))
            {
                BaseCurrency = "ETH";
                MarketCurrency = ticker.symbol.Replace("ETH", "");
            }
        }
    }
}