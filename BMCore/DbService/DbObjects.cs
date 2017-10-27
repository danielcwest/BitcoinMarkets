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
        public decimal BaseExchangeFee;
        public decimal CounterExchangeFee;
        public decimal BaseBaseWithdrawalFee;
        public decimal BaseMarketWithdrawalFee;
        public decimal CounterBaseWithdrawalFee;
        public decimal CounterMarketWithdrawalFee;
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
        public decimal BaseBaseWithdrawalFee;
        public decimal BaseMarketWithdrawalFee;
        public decimal CounterBaseWithdrawalFee;
        public decimal CounterMarketWithdrawalFee;
    }

    public class DbReportTransaction
    {
        public int Id;
        public string BaseExchange;
        public string CounterExchange;
        public string Symbol;
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
        public decimal SpreadThreshold;
        public decimal Commission;
    }

    public class DbOpportunity
    {
        public string BaseExchange;
        public string CounterExchange;
        public string Symbol;
        public decimal Spread;
        public decimal BasePrice;
        public decimal ArbPrice;
        public DateTime CreatedUtc;
        public decimal TradeThreshold;
        public decimal SpreadThreshold;
    }
}