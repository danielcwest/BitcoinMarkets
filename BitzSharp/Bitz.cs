using System;
using System.Threading.Tasks;
using BitzSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Models;
using System.Linq;
using BMCore.Contracts;
using BMCore;
using BMCore.Config;

namespace BitzSharp
{

    public class Bitz : IExchange
    {
        IBitzApi _bitz;
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

        public Task<IEnumerable<ISymbol>> Symbols()
        {
            throw new NotImplementedException();
        }

        public Bitz(ExchangeConfig config)
        {
            _bitz = RestClient.For<IBitzApi>("https://www.bit-z.com");
            name = config.Name;
            fee = config.Fee;
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

        public Task<IAcceptedAction> Buy(string generatedId, string symbol, decimal quantity, decimal rate)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> Sell(string generatedId, string symbol, decimal quantity, decimal rate)
        {
            throw new NotImplementedException();
        }

        public Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICurrencyBalance>> GetBalances()
        {
            throw new NotImplementedException();
        }
    }
}