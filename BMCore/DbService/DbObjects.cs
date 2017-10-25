using System;

namespace BMCore.DbService
{
    public class DbTradeLog
    {
        public string BaseExchange;
        public string ArbExchange;
        public string Symbol;
        public decimal BasePrice;
        public decimal ArbPrice;
        public decimal Spread;
        public DateTime CreatedUtc;
    }

    public class DbOrder
    {
        public long Id; //Client ID (Our ID)
        public long CounterId;
        public int ProcessId;
        public string Exchange;
        public string CounterExchange;
        public string Symbol;
        public string BaseCurrency;
        public string MarketCurrency;
        public string Uuid; //Exchange ID
        public string Status; //Open, Filled, Partial, Canceled, Rejected 
        public decimal Quantity;
        public decimal Price;
        public decimal Rate;
        public decimal Commission; //Fee paid
        public string Side; //Buy or Sell
        public DateTime CreatedUtc;
    }

    public class DbWithdrawOrder
    {
        public long Id; //Client ID (Our ID)
        public string Exchange;
        public string CounterExchange;
        public string Symbol;
        public string Currency;
        public string Uuid; //Exchange ID
        public string Status; //Open, Filled, Partial, Canceled, Rejected 
        public decimal Quantity;
        public decimal Price;
        public decimal Rate;
        public string Side; //Buy or Sell
    }

    public class DbWithdrawal
    {
        public long Id; //Client ID (Our ID)
        public long CounterId;
        public int ProcessId;
        public long OrderId;
        public string FromExchange;
        public string Currency;
        public string Status;
        public string Uuid; //Exchange ID
        public decimal AmountRequested;
        public decimal AmountActual;
        public string TxId;
        public DateTime CreatedUtc;
    }

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