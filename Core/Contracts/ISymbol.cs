using System;

namespace Core.Contracts
{
    public interface ISymbol
    {
        string LocalSymbol { get; set; }
        string ExchangeSymbol { get; set; }
        string BaseCurrency { get; set; }
        string MarketCurrency { get; set; }
    }
}