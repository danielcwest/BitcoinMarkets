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
}