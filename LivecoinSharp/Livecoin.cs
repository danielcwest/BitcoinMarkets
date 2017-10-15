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

        public Livecoin(ConfigExchange config)
        {
            _livecoin = RestClient.For<ILivecoinApi>("https://api.livecoin.net/");
            name = config.Name;
            fee = config.Fee;
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
            sum.symbol = GetLivecoinSymbolFromBMSymbol(symbol);
            return new Market(sum);
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

        public Task<IEnumerable<ISymbol>> Symbols()
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
    }
}