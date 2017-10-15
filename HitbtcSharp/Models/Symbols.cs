using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCore.Contracts;

namespace HitbtcSharp.Models
{
    public class SymbolV1
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        ///Price step parameter
        /// </summary>
        public string step { get; set; }

        /// <summary>
        /// Lot size parameter
        /// </summary>
        public string lot { get; set; }

        /// <summary>
        /// Value of this symbol
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Second value of this symbol
        /// </summary>
        public string commodity { get; set; }

        /// <summary>
        /// Liquidity taker fee
        /// </summary>
        public string takeLiquidityRate { get; set; }

        /// <summary>
        /// Liquidity provider rebate
        /// </summary>
        public string provideLiquidityRate { get; set; }
    }

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

        public Symbol(SymbolV1 symbol)
        {
            ExchangeSymbol = symbol.symbol;
            MarketCurrency = symbol.commodity;
            BaseCurrency = symbol.currency;
            Fee = decimal.Parse(symbol.lot);
            LocalSymbol = symbol.symbol;
        }

        public Symbol(SymbolV2 symbol)
        {
            ExchangeSymbol = symbol.id;
            MarketCurrency = symbol.quoteCurrency;
            BaseCurrency = symbol.baseCurrency;
            Fee = symbol.provideLiquidityRate;
            LocalSymbol = symbol.id;
        }
    }

    public class Symbols
    {
        public List<SymbolV1> symbols { get; set; }
    }
}
