using System;

namespace BMCore.DbService
{
    public class DbArbitragePair
    {
        public int Id;
        public string BaseExchange;
        public string CounterExchange;
        public string Symbol;
        public string BaseSymbol;
        public string CounterSymbol;
        public string BaseCurrency;
        public string MarketCurrency;
        public DateTime LastRunUtc;
        public decimal TradeThreshold;
        public decimal SpreadThreshold;
        public decimal ExchangeFees;
    }

    public class DbTransaction
    {
        public int Id;
        public string BaseExchange;
        public string CounterExchange;
        public string BaseSymbol;
        public string CounterSymbol;
        public string BaseCurrency;
        public string MarketCurrency;
        public string BaseOrderUuid;
        public string CounterOrderUuid;
        public string BaseWithdrawalUuid;
        public string CounterWithdrawalUuid;
        public string BaseTxId;
        public string CounterTxId;
        public string Type;
        public decimal TradeThreshold;
    }
}