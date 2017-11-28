using BittrexSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using RestEase;
using Core.Models;
using Core.Contracts;
using System.Threading.Tasks;
using Core.Config;
using System.IO;
using Newtonsoft.Json;

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


        public Bittrex(ExchangeConfig config)
        {
            _bittrex = new BittrexApi(config.ApiKey, config.Secret);
            name = config.Name;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var markets = await _bittrex.GetMarkets();
            return markets.Select(m => new Symbol(m));
        }

        public async Task<IEnumerable<ITicker>> MarketSummaries()
        {
            var summaries = await _bittrex.GetMarketSummaries();
            return summaries.Select(s => new BMMarket(s));
        }

        public async Task<ITicker> Ticker(string symbol)
        {
            if (!symbol.Contains("-"))
                symbol = GetMarketNameFromSymbol(symbol);
            var sum = await _bittrex.GetMarketSummary(symbol);
            return new BMMarket(sum.FirstOrDefault());
        }

        public async Task<Core.Models.OrderBook> OrderBook(string symbol)
        {
            var ob = new Core.Models.OrderBook(symbol);
            var books = await _bittrex.GetOrderBook(GetMarketNameFromSymbol(symbol), "both", 1);

            if (books.Sell != null && books.Buy != null)
            {
                ob.asks = books.Sell.Select(s => new Core.Models.OrderBookEntry() { price = s.Rate, quantity = s.Quantity }).Take(25).ToList();
                ob.bids = books.Buy.Select(s => new Core.Models.OrderBookEntry() { price = s.Rate, quantity = s.Quantity }).Take(25).ToList();

                ob.asks = Core.Helper.SumOrderEntries(ob.asks);
                ob.bids = Core.Helper.SumOrderEntries(ob.bids);
            }

            return ob;
        }

        public async Task<IAcceptedAction> LimitBuy(string symbol, decimal quantity, decimal rate)
        {
            if (!symbol.Contains("-"))
                symbol = GetMarketNameFromSymbol(symbol);
            return await _bittrex.BuyLimit(symbol, quantity, rate);
        }

        public async Task<IAcceptedAction> LimitSell(string symbol, decimal quantity, decimal rate)
        {
            if (!symbol.Contains("-"))
                symbol = GetMarketNameFromSymbol(symbol);
            return await _bittrex.SellLimit(symbol, quantity, rate);
        }

        public async Task CancelOrder(string orderId)
        {
            await _bittrex.CancelOrder(orderId);
        }

        public async Task<IOrder> CheckOrder(string uuid, string symbol = "")
        {
            var bOrder = await _bittrex.GetOrder(uuid);
            //File.WriteAllText("bittrex_order.json", JsonConvert.SerializeObject(bOrder));

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

        public async Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            var history = await _bittrex.GetWithdrawalHistory();
            var tx = history.Where(h => h.Uuid == uuid).FirstOrDefault();
            //File.WriteAllText("bittrex_withdrawal.json", JsonConvert.SerializeObject(tx));

            return tx;
        }

        public async Task<IEnumerable<HistoricWithdrawal>> GetWithdrawals()
        {
            return await _bittrex.GetWithdrawalHistory();
        }

        public async Task<IEnumerable<HistoricDeposit>> GetDeposits()
        {
            return await _bittrex.GetDepositHistory();
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
            var balances = await _bittrex.GetBalances();
            return balances.Select(b => new CurrencyBalance(b));
        }

        Task<IEnumerable<string>> IExchange.CancelOrders(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> MarketBuy(string symbol, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> MarketSell(string symbol, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeOrderbook(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> FillOrKill(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public ISocketExchange GetSocket()
        {
            throw new NotImplementedException();
        }
    }
}