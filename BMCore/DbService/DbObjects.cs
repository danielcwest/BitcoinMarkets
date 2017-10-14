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
        public DateTime CreatedAtUtc;
    }

    public class DbOrder
    {
        public long Id; //Client ID (Our ID)
        public long CounterId;
        public string Exchange;
        public string Symbol;
        public string BaseCurrency;
        public string MarketCurrency;
        public string Uuid; //Exchange ID
        public string Status; //Open, Filled, Partial, Canceled, Rejected 
        public decimal Quantity;
        public decimal Price;
        public decimal Commission; //Fee paid
        public string Side; //Buy or Sell
        public DateTime CreatedUtc;
    }
}