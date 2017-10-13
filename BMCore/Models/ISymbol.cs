using System;

namespace BMCore.Models
{
    public interface ISymbol
    {
        string LocalSymbol { get; set; }
        string ExchangeSymbol { get; set; }
        string BaseCurrency { get; set; }
        string MarketCurrency { get; set; }
        decimal LotSize { get; set; }
    }
}