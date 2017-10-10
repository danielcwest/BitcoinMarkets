using System;
using System.Threading.Tasks;
using LivecoinSharp.Models;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using BMCore.Contracts;
using BMCore;

namespace LivecoinSharp
{
    public class Livecoin : IExchange
    {

        ILivecoinApi _livecoin;

        public Livecoin()
        {
            _livecoin = RestClient.For<ILivecoinApi>("https://api.livecoin.net/");
        }

        public async Task<IEnumerable<Ticker>> Tickers()
        {
            return await _livecoin.GetTickers();
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _livecoin.GetTickers();
            return summaries.Where(s => s.symbol.EndsWith("/BTC") || s.symbol.EndsWith("/ETH")).Select(s => new Market(s));
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _livecoin.GetOrderBook(GetLivecoinSymbolFromBMSymbol(symbol));

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var sum = await _livecoin.GetTicker(GetLivecoinSymbolFromBMSymbol(symbol));
            return new Market(sum);
        }

        public decimal GetFee()
        {
            return 0.0018M;
        }

        public string GetExchangeName()
        {
            return "Livecoin";
        }

        private string GetLivecoinSymbolFromBMSymbol(string bmSymbol)
        {
            string symbol = "";
            if (bmSymbol.EndsWith("BTC"))
            {
                var c = bmSymbol.Replace("BTC", "");
                symbol = string.Format("{0}/BTC", c);
            }
            else if (bmSymbol.EndsWith("ETH"))
            {
                var c = bmSymbol.Replace("ETH", "");
                symbol = string.Format("{0}/ETH", c);
            }
            return symbol;
        }
    }
}