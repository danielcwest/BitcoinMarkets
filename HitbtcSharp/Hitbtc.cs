using System;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using System.Threading.Tasks;
using HitbtcSharp.Models;
using BMCore.Contracts;
using BMCore;
using System.Security.Cryptography;
using System.Text;

namespace HitbtcSharp
{
    public class Hitbtc : IExchange
    {
        IHitbtcApi _hitbtc;
        ConfigExchange _config;
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

        public Hitbtc(ConfigExchange config)
        {
            _hitbtc = RestClient.For<IHitbtcApi>("http://api.hitbtc.com");
            _config = config;
            name = config.Name;
            fee = config.Fee;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _hitbtc.GetTickers();
            return summaries.Where(s => s.Key.EndsWith("BTC") || s.Key.EndsWith("ETH")).Select(s => new Market(s.Key, s.Value));
        }

        public async Task<IMarket> MarketSummary(string symbol)
        {
            var summary = await _hitbtc.GetTicker(symbol);
            return new Market(symbol, summary);
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _hitbtc.GetOrderBook(symbol);

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = e[0], quantity = e[1] }).Take(25).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<IAcceptedAction> Buy(string symbol, decimal quantity, decimal rate)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/new_order?nonce={0}&apikey={1}", nonce, _config.ApiKey);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var order = new OrderRequest()
            {
                clientOrderId = "",
                price = rate,
                symbol = symbol,
                side = "buy",
                quantity = Convert.ToInt32(quantity),
                type = "market"
            };


            var execution = await _hitbtc.PlaceOrder(sign, nonce, _config.ApiKey, order);

            return execution.ExecutionReport;
        }

        public async Task CancelOrder(string orderId)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/cancel_order?nonce={0}&apikey={1}", nonce, _config.ApiKey);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var order = new CancelOrderRequest()
            {
                clientOrderId = ""
            };

            await _hitbtc.CancelOrder(sign, nonce, _config.ApiKey, order);
        }

        public async Task<IAcceptedAction> Sell(string symbol, decimal quantity, decimal rate)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/new_order?nonce={0}&apikey={1}", nonce, _config.ApiKey);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var order = new OrderRequest()
            {
                clientOrderId = "",
                price = rate,
                symbol = symbol,
                side = "sell",
                quantity = Convert.ToInt32(quantity),
                type = "market"
            };


            var execution = await _hitbtc.PlaceOrder(sign, nonce, _config.ApiKey, order);
            return execution.ExecutionReport;
        }

        public async Task<IOrder> CheckOrder(string orderId)
        {
            int limit = 25;
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/orders/recent?nonce={0}&apikey={1}&max_results={2}", nonce, _config.ApiKey, limit);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var orders = await _hitbtc.GetOrders(sign, nonce, _config.ApiKey, limit);
            var order = orders.orders.Find(o => o.orderId == orderId);

            return new Order(order);
        }

        public async Task<IEnumerable<IOrder>> GetOrders()
        {
            int limit = 25;
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/orders/recent?nonce={0}&apikey={1}&max_results={2}", nonce, _config.ApiKey, limit);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var orders = await _hitbtc.GetOrders(sign, nonce, _config.ApiKey, limit);
            return orders.orders.Select(o => new Order(o));
        }

        public async Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/payout?nonce={0}&apikey={1}&amount={2}&currency_code={3}&address={4}", nonce, _config.ApiKey, quantity, currency, address);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);
            return await _hitbtc.Withdraw(sign, nonce, _config.ApiKey, quantity, currency, address);
        }

        public async Task<IDepositAddress> GetDepositAddress(string currency)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/address/{0}?nonce={1}&apikey={2}", currency, nonce, _config.ApiKey);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);
            var address = await _hitbtc.GetAddress(sign, nonce, _config.ApiKey, currency);
            return new DepositAddress() { Currency = currency, Address = address.address };
        }

        public async Task<ICurrencyBalance> GetBalance(string currency)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/balance?nonce={0}&apikey={1}", nonce, _config.ApiKey);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var multiBalance = await _hitbtc.GetTradingBalances(sign, nonce, _config.ApiKey);

            return multiBalance.balance.Where(b => b.Currency == currency).FirstOrDefault();
        }

        //Type: trading or payment
        public async Task<IEnumerable<ICurrencyBalance>> GetBalances(string type, bool filterZero = true)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/{0}/balance?nonce={1}&apikey={2}", type, nonce, _config.ApiKey);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            MultiCurrencyBalance multiBalance;

            if (type == "trading")
                multiBalance = await _hitbtc.GetTradingBalances(sign, nonce, _config.ApiKey);
            else
                multiBalance = await _hitbtc.GetMainBalances(sign, nonce, _config.ApiKey);

            if (filterZero)
                return multiBalance.balance.Where(b => b.Balance > 0);
            else
                return multiBalance.balance;
        }

        private static string GetClientId()
        {
            return "";
        }

        private static long GetNonce()
        {
            return DateTime.Now.Ticks * 10 / TimeSpan.TicksPerMillisecond; // use millisecond timestamp or whatever you want
        }

        private static string CalculateSignature(string text, string secretKey)
        {
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(text));
                return string.Concat(hmacsha512.Hash.Select(b => b.ToString("x2")).ToArray()); // minimalistic hex-encoding and lower case
            }
        }
    }
}
