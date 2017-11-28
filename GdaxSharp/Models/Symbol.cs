using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Contracts;

namespace GdaxSharp.Models
{
    public class Product
    {
        public string id { get; set; }
        public string base_currency { get; set; }
        public string quote_currency { get; set; }
        public decimal base_min_size { get; set; }
        public decimal base_max_size { get; set; }
        public decimal quote_increment { get; set; }
    }

    public class Symbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }

        public Symbol(Product p)
        {
            ExchangeSymbol = p.id;
            LocalSymbol = p.base_currency + p.quote_currency;
            BaseCurrency = p.quote_currency;
            MarketCurrency = p.base_currency;
        }
    }
}