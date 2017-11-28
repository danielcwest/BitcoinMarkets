using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Core.Models
{
    public class OrderBook
    {
        public long updateId { get; set; }
        public string symbol { get; set; }
        public List<OrderBookEntry> bids { get; set; }
        public List<OrderBookEntry> asks { get; set; }

        public OrderBook(string symbol)
        {
            this.symbol = symbol;
        }
    }

    public class OrderBookCache
    {
        public string Symbol { get; set; }

        public ConcurrentDictionary<decimal, OrderBookEntry> Bids {get;set;}
        public ConcurrentDictionary<decimal, OrderBookEntry> Asks { get; set; }

        public OrderBookCache(OrderBook book)
        {
            Symbol = book.symbol;
            Bids = new ConcurrentDictionary<decimal, OrderBookEntry>(book.bids.ToDictionary(b => b.price));
            Asks = new ConcurrentDictionary<decimal, OrderBookEntry>(book.asks.ToDictionary(b => b.price));
        }

        public OrderBook ToOrderBook()
        {
            return new OrderBook(this.Symbol)
            {
                asks = this.Asks.Values.OrderBy(v => v.price).Take(25).ToList(),
                bids = this.Bids.Values.OrderByDescending(v => v.price).Take(25).ToList()
            };
        }

    }
}