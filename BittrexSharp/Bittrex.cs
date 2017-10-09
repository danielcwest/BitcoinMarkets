using BittrexSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using RestEase;
using BMCore.Models;
using BMCore.Contracts;
using System.Threading.Tasks;

namespace BittrexSharp
{

    public class Bittrex : IExchange
    {
        BittrexApi _bittrex;
        public Bittrex(string apiKey, string apiSecret)
        {
            _bittrex = new BittrexApi(apiKey, apiSecret);
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _bittrex.GetMarketSummaries();
            return summaries.Where(s => s.MarketName.StartsWith("BTC-") || s.MarketName.StartsWith("ETH-")).Select(s => new BMMarket(s));
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var sum = await _bittrex.GetMarketSummary(Helper.GetMarketNameFromSymbol(symbol));
            return new BMMarket(sum.FirstOrDefault());
        }

        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            var ob = new BMCore.Models.OrderBook(symbol);
            var books = await _bittrex.GetOrderBook(Helper.GetMarketNameFromSymbol(symbol), "both", 1);

            if (ob.symbol != Helper.GetSymbolFromMarketName(books.MarketName))
            {
                throw new Exception();
            }

            ob.asks = books.Sell.Select(s => new BMCore.Models.OrderBookEntry() { price = s.Rate, quantity = s.Quantity }).Take(25).ToList();
            ob.bids = books.Buy.Select(s => new BMCore.Models.OrderBookEntry() { price = s.Rate, quantity = s.Quantity }).Take(25).ToList();

            ob.asks = BMCore.Helper.SumOrderEntries(ob.asks);
            ob.bids = BMCore.Helper.SumOrderEntries(ob.bids);

            return ob;
        }
    }
}