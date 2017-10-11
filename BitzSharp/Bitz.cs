using System;
using System.Threading.Tasks;
using BitzSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Models;
using System.Linq;
using BMCore.Contracts;
using BMCore;

namespace BitzSharp
{

    public class Bitz : IExchange
    {
        IBitzApi _bitz;

        public Bitz(string apiKey, string apiSecret)
        {
            _bitz = RestClient.For<IBitzApi>("https://www.bit-z.com");
        }

        public async Task<Dictionary<string, Ticker>> Tickers()
        {
            var tickers = await _bitz.GetTickers();
            return tickers.data;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _bitz.GetTickers();
            return summaries.data.Where(s => s.Key.EndsWith("_btc") || s.Key.EndsWith("_eth")).Select(s => new Market(s.Key, s.Value));
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            OrderBook book = null;
            var response = await _bitz.GetOrderBook(GetBitzSymbolFromBMSymbol(symbol));
            Depth depth = response.data;

            if (depth != null && depth.asks != null && depth.bids != null)
            {
                book = new OrderBook(symbol);
                book.asks = new List<OrderBookEntry>();
                book.bids = new List<OrderBookEntry>();

                book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();
                book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();

                book.asks = Helper.SumOrderEntries(book.asks);
                book.bids = Helper.SumOrderEntries(book.bids);
            }

            return book;
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var bitzSymbol = GetBitzSymbolFromBMSymbol(symbol);
            var res = await _bitz.GetTicker(bitzSymbol);
            Market market = null;
            if (res.data != null)
                market = new Market(bitzSymbol, res.data);
            return market;
        }

        public decimal GetFee()
        {
            return 0.008M;
        }

        public string GetExchangeName()
        {
            return "Bitz";
        }

        private string GetBitzSymbolFromBMSymbol(string bmSymbol)
        {
            string symbol = "";
            if (bmSymbol.EndsWith("BTC"))
            {
                var c = bmSymbol.Replace("BTC", "");
                symbol = string.Format("{0}_btc", c.ToLowerInvariant());
            }
            else if (bmSymbol.EndsWith("ETH"))
            {
                var c = bmSymbol.Replace("ETH", "");
                symbol = string.Format("{0}_eth", c.ToLowerInvariant());
            }
            return symbol;
        }
    }
}