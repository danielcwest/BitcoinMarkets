using System;
using System.Collections.Generic;
using System.Linq;
using Core.Config;
using Core.Contracts;
using Core.Util;
using Newtonsoft.Json;

namespace Core.Models
{
    public class ArbitrageMarket
    {
        public string Symbol { get; set; }
        public IMarket baseMarket { get; set; }
        public OrderBook baseBook { get; set; }
        public IMarket arbitrageMarket { get; set; }
        public OrderBook arbitrageBook { get; set; }

        public CurrencyConfig baseCurrency { get; set; }

        public ArbitrageMarket(IMarket baseM, OrderBook baseBook, IMarket arbM, OrderBook arbBook, CurrencyConfig baseCurrency)
        {
            if (baseM == null || baseBook == null || arbM == null || arbBook == null)
                throw new Exception("Market data was null");

            this.Symbol = baseM.MarketName;
            this.baseMarket = baseM;
            this.baseBook = baseBook;
            this.arbitrageMarket = arbM;
            this.arbitrageBook = arbBook;
            this.baseCurrency = baseCurrency;
        }
    }
}