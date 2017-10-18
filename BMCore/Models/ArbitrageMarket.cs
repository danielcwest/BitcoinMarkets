using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Contracts;
using BMCore.Util;
using Newtonsoft.Json;

namespace BMCore.Models
{
    public class ArbitrageMarket
    {
        public string Symbol { get; set; }
        public IMarket baseMarket { get; set; }
        public OrderBook baseBook { get; set; }
        public IMarket arbitrageMarket { get; set; }
        public OrderBook arbitrageBook { get; set; }

        public decimal spreadThreshold { get; set; }

        public ArbitrageMarket(IMarket baseM, OrderBook baseBook, IMarket arbM, OrderBook arbBook, decimal spreadThreshold)
        {
            if (baseM == null || baseBook == null || arbM == null || arbBook == null)
                throw new Exception("Market data was null");

            this.Symbol = baseM.MarketName;
            this.baseMarket = baseM;
            this.baseBook = baseBook;
            this.arbitrageMarket = arbM;
            this.arbitrageBook = arbBook;
            this.spreadThreshold = spreadThreshold;
        }
    }
}