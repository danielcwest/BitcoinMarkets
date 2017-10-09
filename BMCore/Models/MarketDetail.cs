using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using Newtonsoft.Json;

namespace BMCore.Models
{
    public class MarketDetail
    {
        public IMarket market { get; set; }
        public OrderBook orderBook { get; set; }

        public MarketDetail() { }
        public MarketDetail(IMarket market, OrderBook book)
        {
            this.market = market;
            this.orderBook = book;
        }
    }
}