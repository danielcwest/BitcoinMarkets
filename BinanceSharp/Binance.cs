using System;
using System.Threading.Tasks;
using BinanceSharp.Models;
using System.Collections.Generic;
using RestEase;
using BMCore.Models;
using System.Linq;
using BMCore.Contracts;
using BMCore;

namespace BinanceSharp
{

    public class Binance : IExchange
    {
        IBinanceApi _binance;
        public Binance()
        {
            _binance = RestClient.For<IBinanceApi>("https://www.binance.com");
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var prices = await _binance.GetPrices();

            var pricesDic = prices.ToDictionary(p => p.symbol);

            var books = await _binance.GetBooks();

            var tickersDic = books.ToDictionary(b => b.symbol);

            return pricesDic.Keys.Select(k => new Market(pricesDic[k], tickersDic[k]));
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _binance.GetOrderBook(symbol);

            book.asks = depth.asks.Select((a, index) => new OrderBookEntry() { price = Convert.ToDecimal(a[0]), quantity = Convert.ToDecimal(a[1]) }).Take(25).ToList();
            book.bids = depth.bids.Select((a, index) => new OrderBookEntry() { price = Convert.ToDecimal(a[0]), quantity = Convert.ToDecimal(a[1]) }).Take(25).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var ticker = await _binance.GetTicker(symbol);
            return new Market(symbol, ticker);
        }

        public decimal GetFee()
        {
            return 0.0025M;
        }

        public string GetExchangeName()
        {
            return "Binance";
        }
    }

}