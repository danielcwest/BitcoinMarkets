using System;

namespace Core.DbService
{
    public class DbArbitragePair
    {
        public int Id;
        public string BaseExchange;
        public string CounterExchange;
        public string Symbol;
        public string Status;
        public string Type;
        public string BaseSymbol;
        public string CounterSymbol;
        public string BaseCurrency;
        public string MarketCurrency;
        public decimal TradeThreshold;
        public decimal SpreadThreshold;
        public decimal WithdrawalThreshold;
        public decimal BaseExchangeFee;
        public decimal CounterExchangeFee;
        public decimal BaseBaseWithdrawalFee;
        public decimal BaseMarketWithdrawalFee;
        public decimal CounterBaseWithdrawalFee;
        public decimal CounterMarketWithdrawalFee;
        public decimal AskSpread;
        public decimal BidSpread;
        public decimal MarketSpread;
        public int DecimalPlaces;
        public decimal AskMultiplier;
        public decimal BidMultiplier;
        public decimal Increment;
        public decimal TickSize;
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

    public class DbMakerOrder
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
        public string Type;
        public decimal TradeThreshold;
        public decimal WithdrawalThreshold;
        public decimal CounterExchangeFee;
        public decimal CounterBaseWithdrawalFee;
        public decimal CounterMarketWithdrawalFee;
        public decimal BaseQuantityFilled;
        public decimal CounterQuantityFilled;
        public decimal AskSpread;
        public decimal BidSpread;
        public int ProcessId;
        public decimal Commission;
    }

    public class DbBalance
    {
        public string Currency;
        public string BaseExchange;
        public string CounterExchange;
        public decimal Total;
        public decimal BtcValue;
        public int ProcessId;
        public DateTime CreatedUtc;
    }

    public class DbHeroStat
    {
        public string Symbol;
        public decimal Commission;
        public int TradeCount;
    }

}