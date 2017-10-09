using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Util;
using Newtonsoft.Json;

namespace BMCore.Models
{
    public class OrderBook
    {
        public string symbol { get; set; }
        public List<OrderBookEntry> bids { get; set; }
        public List<OrderBookEntry> asks { get; set; }

        public OrderBook(string symbol)
        {
            this.symbol = symbol;
        }
    }
}