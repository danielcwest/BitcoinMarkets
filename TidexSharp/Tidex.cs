using System;
using System.Threading.Tasks;
using TidexSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Models;
using System.Linq;
using System.Text;
using BMCore.Contracts;
using BMCore;

namespace TidexSharp
{
    public class Tidex : IExchange
    {
        ITidexApi _tidex;
        public Tidex(string apiKey, string apiSecret)
        {
            _tidex = RestClient.For<ITidexApi>("https://api.tidex.com");
        }

        public async Task<IEnumerable<object>> Info()
        {
            var summaries = await _tidex.Info();
            return summaries.pairs.Keys;
        }

        public async Task<Dictionary<string, Ticker>> Pairs()
        {
            var pairs = await _tidex.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _tidex.GetTickers(sb.ToString());

            return summaries;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var pairs = await _tidex.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _tidex.GetTickers(sb.ToString());

            return summaries.Select(s => new Market(s.Key, s.Value));
        }



        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            string tidexSymbol = GetTidexSymbolFromBMSymbol(symbol);
            var dic = await _tidex.GetOrderBook(tidexSymbol);
            var depth = dic[tidexSymbol];

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            string tidexSymbol = GetTidexSymbolFromBMSymbol(symbol);
            var dic = await _tidex.GetTickers(tidexSymbol);
            return new Market(tidexSymbol, dic[tidexSymbol]);
        }

        public decimal GetFee()
        {
            return 0.0010M;
        }

        public string GetExchangeName()
        {
            return "Tidex";
        }

        private string GetTidexSymbolFromBMSymbol(string bmSymbol)
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