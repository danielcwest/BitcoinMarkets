using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCore.Models;

namespace HitbtcSharp.Models
{
    public class HitSymbol
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

    public class Symbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public decimal LotSize { get; set; }

        public Symbol(HitSymbol symbol)
        {
            ExchangeSymbol = symbol.symbol;
            MarketCurrency = symbol.commodity;
            BaseCurrency = symbol.currency;
            LotSize = decimal.Parse(symbol.lot);
            LocalSymbol = symbol.symbol;
        }
    }

    public class Symbols
    {
        public List<HitSymbol> symbols { get; set; }
    }
}
