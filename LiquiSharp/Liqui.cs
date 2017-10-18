using System;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using System.Threading.Tasks;
using System.Text;
using LiquiSharp.Models;
using BMCore.Contracts;
using BMCore;
using BMCore.Config;

namespace LiquiSharp
{
    public class Liqui : IExchange
    {
        ILiquiApi _liqui;
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

        public Liqui(ExchangeConfig config)
        {
            _liqui = RestClient.For<ILiquiApi>("https://api.liqui.io");
            name = config.Name;
            fee = config.Fee;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var summaries = await _liqui.Info();
            return summaries.pairs.Select(kvp => new Symbol(kvp.Key, kvp.Value));
        }

        public async Task<Dictionary<string, Ticker>> Pairs()
        {
            var pairs = await _liqui.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _liqui.GetTickers(sb.ToString());

            return summaries;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var pairs = await _liqui.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _liqui.GetTickers(sb.ToString());

            return summaries.Select(s => new Market(s.Key, s.Value));
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            string liquiSymbol = GetLiquiSymbolFromBMSymbol(symbol);
            var dic = await _liqui.GetOrderBook(liquiSymbol);
            var depth = dic[liquiSymbol];

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            string liquiSymbol = GetLiquiSymbolFromBMSymbol(symbol);
            var dic = await _liqui.GetTickers(liquiSymbol);
            return new Market(liquiSymbol, dic[liquiSymbol]);
        }

        private string GetLiquiSymbolFromBMSymbol(string bmSymbol)
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
