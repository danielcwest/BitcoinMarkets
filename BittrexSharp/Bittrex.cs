using BittrexSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using RestEase;
using BMCore.Models;
using BMCore.Contracts;
using System.Threading.Tasks;
using BMCore.Config;

namespace BittrexSharp
{

    public class Bittrex : IExchange
    {
        BittrexApi _bittrex;

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

        public Bittrex(ExchangeConfig config)
        {
            _bittrex = new BittrexApi(config.ApiKey, config.Secret);
            name = config.Name;
            fee = config.Fee;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var markets = await _bittrex.GetMarkets();
            return markets.Where(m => m.BaseCurrency == "BTC" || m.BaseCurrency == "ETH").Select(m => new Symbol(m));
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _bittrex.GetMarketSummaries();
            return summaries.Where(s => s.MarketName.StartsWith("BTC-") || s.MarketName.StartsWith("ETH-")).Select(s => new BMMarket(s));
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var sum = await _bittrex.GetMarketSummary(GetMarketNameFromSymbol(symbol));
            return new BMMarket(sum.FirstOrDefault());
        }

        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            var ob = new BMCore.Models.OrderBook(symbol);
            var books = await _bittrex.GetOrderBook(GetMarketNameFromSymbol(symbol), "both", 1);

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

        public async Task<IAcceptedAction> Buy(string generatedId, string symbol, decimal quantity, decimal rate)
        {
            return await _bittrex.BuyLimit(GetMarketNameFromSymbol(symbol), quantity, rate);
        }

        public async Task<IAcceptedAction> Sell(string generatedId, string symbol, decimal quantity, decimal rate)
        {
            if (!symbol.Contains("-"))
                symbol = GetMarketNameFromSymbol(symbol);
            return await _bittrex.SellLimit(symbol, quantity, rate);
        }

        public async Task CancelOrder(string orderId)
        {
            await _bittrex.CancelOrder(orderId);
        }

        public async Task<IOrder> CheckOrder(string uuid)
        {
            var bOrder = await _bittrex.GetOrder(uuid);
            return new Order(bOrder);
        }

        public async Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            return await _bittrex.Withdraw(currency, quantity, address, paymentId);
        }

        public async Task<IDepositAddress> GetDepositAddress(string currency)
        {
            return await _bittrex.GetDepositAddress(currency);
        }

        public async Task<ICurrencyBalance> GetBalance(string currency)
        {
            return await _bittrex.GetBalance(currency);
        }

        public async Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            var history = await _bittrex.GetWithdrawalHistory();
            return history.Where(h => h.Uuid == uuid).FirstOrDefault();
        }

        //return Bittrex market name in the form of BTC-XMR or ETH-XRP
        private static string GetMarketNameFromSymbol(string symbol)
        {
            if (symbol.EndsWith("BTC"))
            {
                return string.Format("BTC-{0}", symbol.Replace("BTC", ""));
            }
            else if (symbol.EndsWith("ETH"))
            {
                return string.Format("ETH-{0}", symbol.Replace("ETH", ""));
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ICurrencyBalance>> GetBalances()
        {
            return await _bittrex.GetBalances();
        }
    }
}