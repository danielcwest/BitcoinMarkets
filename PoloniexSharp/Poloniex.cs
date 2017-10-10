using System;
using System.Threading.Tasks;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using BMCore.Contracts;
using PoloniexSharp.Models;
using BMCore;

namespace PoloniexSharp
{
    public class Poloniex : IExchange
    {
        IPoloniexApi _poloniex;

        public Poloniex()
        {
            _poloniex = RestClient.For<IPoloniexApi>("https://poloniex.com");
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _poloniex.GetTickers();
            return summaries.Where(s => s.Key.StartsWith("BTC") || s.Key.StartsWith("ETH")).Select(s => new Market(s.Key, s.Value));
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _poloniex.GetOrderBook(GetMarketNameFromSymbol(symbol));

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public static string GetMarketNameFromSymbol(string symbol)
        {
            if (symbol.EndsWith("BTC"))
            {
                return string.Format("BTC_{0}", symbol.Replace("BTC", ""));
            }
            else if (symbol.EndsWith("ETH"))
            {
                return string.Format("ETH_{0}", symbol.Replace("ETH", ""));
            }
            else
            {
                return null;
            }
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {

            var summaries = await _poloniex.GetTickers();
            var poloSymbol = GetMarketNameFromSymbol(symbol);
            var ticker = summaries[poloSymbol];

            return new Market(poloSymbol, ticker);
        }

        public decimal GetFee()
        {
            return 0.0025M;
        }

        public string GetExchangeName()
        {
            return "Poloniex";
        }
    }
}
