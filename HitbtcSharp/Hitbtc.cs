using System;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using System.Threading.Tasks;
using HitbtcSharp.Models;
using BMCore.Contracts;
using BMCore;

namespace HitbtcSharp
{
    public class Hitbtc : IExchange
    {
        IHitbtcApi _hitbtc;
        public Hitbtc()
        {
            _hitbtc = RestClient.For<IHitbtcApi>("http://api.hitbtc.com");
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _hitbtc.GetTickers();
            return summaries.Where(s => s.Key.EndsWith("BTC") || s.Key.EndsWith("ETH")).Select(s => new Market(s.Key, s.Value));
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var summary = await _hitbtc.GetTicker(symbol);
            return new Market(symbol, summary);
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _hitbtc.GetOrderBook(symbol);

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }
    }
}
