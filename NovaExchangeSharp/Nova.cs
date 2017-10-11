using System;
using System.Threading.Tasks;
using NovaExchangeSharp.Models;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using BMCore.Contracts;
using BMCore;

namespace NovaExchangeSharp
{
    public class Nova : IExchange
    {
        INovaExchangeApi _nova;

        public Nova(string apiKey, string apiSecret)
        {
            _nova = RestClient.For<INovaExchangeApi>("https://novaexchange.com");
        }

        public string GetExchangeName()
        {
            return "NovaExchange";
        }

        public decimal GetFee()
        {
            return 0.0020M;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summary = await _nova.GetTickers();
            return summary.markets.Where(s => s.basecurrency.Equals("ETH") || s.basecurrency.Equals("BTC")).Select(s => new Market(s));
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var summary = await _nova.GetTicker(GetNovaSymbolFromBMSymbol(symbol));
            Market market = null;
            if (summary.markets != null)
                market = new Market(summary.markets.FirstOrDefault());

            return market;
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _nova.GetOrderBook(GetNovaSymbolFromBMSymbol(symbol));

            book.asks = depth.sellorders.Select(e => new OrderBookEntry() { price = e.price, quantity = e.amount }).ToList();
            book.bids = depth.buyorders.Select(e => new OrderBookEntry() { price = e.price, quantity = e.amount }).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        private string GetNovaSymbolFromBMSymbol(string bmSymbol)
        {
            string symbol = "";
            if (bmSymbol.EndsWith("BTC"))
            {
                var c = bmSymbol.Replace("BTC", "");
                symbol = string.Format("BTC_{0}", c);
            }
            else if (bmSymbol.EndsWith("ETH"))
            {
                var c = bmSymbol.Replace("ETH", "");
                symbol = string.Format("ETH_{0}", c);
            }
            return symbol;
        }
    }
}