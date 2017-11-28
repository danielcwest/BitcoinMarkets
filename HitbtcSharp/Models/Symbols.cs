using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;

namespace HitbtcSharp.Models
{
    public class SymbolV2
    {
        public string id { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public decimal quantityIncrement { get; set; }
        public decimal tickSize { get; set; }
        public decimal takeLiquidityRate { get; set; }
        public decimal provideLiquidityRate { get; set; }
        public string feeCurrency { get; set; }
    }

    public class Symbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public decimal Fee { get; set; }
        public string FeeCurrency { get; set; }
        public decimal TickSize { get; set; }

        public Symbol(SymbolV2 symbol)
        {
            ExchangeSymbol = symbol.id;
            MarketCurrency = symbol.baseCurrency;
            BaseCurrency = symbol.quoteCurrency;
            Fee = Math.Abs(symbol.provideLiquidityRate);
            LocalSymbol = symbol.id;
            FeeCurrency = symbol.feeCurrency;
            TickSize = symbol.tickSize;
        }
    }
}
