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

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        private decimal fee;
        public decimal Fee
        {
            get
            {
                return fee;
            }
        }

        public Binance(ConfigExchange config)
        {
            _binance = RestClient.For<IBinanceApi>("https://www.binance.com");
            name = config.Name;
            fee = config.Fee;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var prices = await _binance.GetPrices();

            throw new NotImplementedException();
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

        public Task CancelOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IOrder> CheckOrder(string uuid)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDepositAddress> GetDepositAddress(string currency)
        {
            throw new NotImplementedException();
        }

        public Task<ICurrencyBalance> GetBalance(string currency)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> Buy(string symbol, decimal quantity, decimal rate, decimal lot = 1.0M)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> Sell(string symbol, decimal quantity, decimal rate, decimal lot = 1.0M)
        {
            throw new NotImplementedException();
        }
    }

}