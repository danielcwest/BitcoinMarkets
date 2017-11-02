using BMCore.Contracts;
using System;
using BMCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BMCore.Config;
using RestEase;
using System.Text;
using System.Security.Cryptography;
using GdaxSharp.Models;
using BMCore.Util;
using System.Linq;
using BMCore;
using Newtonsoft.Json;
using System.Net.Http;

namespace GdaxSharp
{
    public class Gdax : IExchange
    {
        IGdaxApi _gdax;
        ExchangeConfig _config;
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public Gdax(ExchangeConfig config)
        {
            _config = config;
            name = config.Name;

            _gdax = RestClient.For<IGdaxApi>("https://api.gdax.com");
            _gdax.ApiKey = _config.ApiKey;
            _gdax.PassPhrase = _config.Passphrase;
        }

        #region Public

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var products = await _gdax.Products();
            return products.Select(p => new Symbol(p));
        }

        public async Task<ITicker> Ticker(string symbol)
        {
            string exchangeSymbol = symbol;
            if (!symbol.Contains("-"))
                exchangeSymbol = GetProductIdFromSymbol(symbol);

            var ticker = await _gdax.Ticker(exchangeSymbol);
            return new Market(symbol, ticker);
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);

            if (!symbol.Contains("-"))
                symbol = GetProductIdFromSymbol(symbol);

            var depth = await _gdax.GetOrderBook(symbol);

            book.asks = depth.asks.Select(e => new OrderBookEntry() { price = decimal.Parse(e[0]), quantity = decimal.Parse(e[1]) }).ToList();
            book.bids = depth.bids.Select(e => new OrderBookEntry() { price = decimal.Parse(e[0]), quantity = decimal.Parse(e[1]) }).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }
        #endregion

        #region Account

        public async Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "POST";
            string requestUrl = "/withdrawals/crypto";

            var data = new
            {
                amount = quantity,
                currency = currency,
                crypto_address = address
            };

            string json = JsonConvert.SerializeObject(data);
            string sig = ComputeSignature(ts, httpMethod, requestUrl, json);

            return await _gdax.Withdraw(sig, ts, data);
        }


        public async Task<IEnumerable<CoinbaseAccount>> GetCoinbaseAccounts()
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = "/coinbase-accounts";
            string sig = ComputeSignature(ts, httpMethod, requestUrl);

            return await _gdax.CoinbaseAccounts(sig, ts);
        }

        public async Task<IEnumerable<ICurrencyBalance>> GetBalances()
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = "/accounts";
            string sig = ComputeSignature(ts, httpMethod, requestUrl);

            var accounts = await _gdax.Accounts(sig, ts);
            return accounts.Select(a => new GdaxBalance(a));
        }

        public async Task<IDepositAddress> GetDepositAddress(string currency)
        {
            throw new NotImplementedException();
        }

        public async Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Trade

        public async Task<IAcceptedAction> Buy(string generatedId, string symbol, decimal quantity, decimal price)
        {
            var productId = symbol;
            if (!symbol.Contains("-"))
                productId = GetProductIdFromSymbol(symbol);

            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "POST";
            string requestUrl = "/orders";

            var data = new
            {
                type = "limit",
                side = "buy",
                product_id = productId,
                price = price.ToString("#.####"),
                size = quantity.ToString("#.####"),
                post_only = true,
                time_in_force = "GTT",
                cancel_after = "min"
            };

            string json = JsonConvert.SerializeObject(data);

            string sig = ComputeSignature(ts, httpMethod, requestUrl, json);

            var order = await _gdax.NewOrder(sig, ts, data);
            return new Order(order);
        }

        public async Task<IAcceptedAction> Sell(string generatedId, string symbol, decimal quantity, decimal price)
        {
            var productId = symbol;
            if (!symbol.Contains("-"))
                productId = GetProductIdFromSymbol(symbol);

            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "POST";
            string requestUrl = "/orders";

            var data = new
            {
                type = "limit",
                side = "sell",
                product_id = productId,
                price = price.ToString("#.####"),
                size = quantity.ToString("#.####"),
                post_only = true,
                time_in_force = "GTT",
                cancel_after = "min"
            };

            string json = JsonConvert.SerializeObject(data);
            string sig = ComputeSignature(ts, httpMethod, requestUrl, json);

            var order = await _gdax.NewOrder(sig, ts, data);
            return new Order(order);
        }

        public async Task CancelOrder(string orderId)
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "DELETE";
            string requestUrl = string.Format("/orders/{0}", orderId);
            string sig = ComputeSignature(ts, httpMethod, requestUrl);
            await _gdax.CancelOrder(sig, ts, orderId);
        }

        public async Task<IEnumerable<string>> CancelOrders(string symbol)
        {
            if (!symbol.Contains("-"))
                symbol = GetProductIdFromSymbol(symbol);

            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "DELETE";
            string requestUrl = string.Format("/orders?product_id={0}", symbol);
            string sig = ComputeSignature(ts, httpMethod, requestUrl);
            return await _gdax.CancelOrders(sig, ts, symbol);

        }

        public async Task<IOrder> CheckOrder(string uuid)
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = string.Format("/orders/{0}", uuid);
            string sig = ComputeSignature(ts, httpMethod, requestUrl);
            var order = await _gdax.Order(sig, ts, uuid);
            return new Order(order);
        }

        public async Task<GdaxOrder> GetOrder(string uuid)
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = string.Format("/orders/{0}", uuid);
            string sig = ComputeSignature(ts, httpMethod, requestUrl);
            return await _gdax.Order(sig, ts, uuid);
        }

        public async Task<IEnumerable<GdaxOrder>> GetOrders()
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = "/orders?status=all";
            string sig = ComputeSignature(ts, httpMethod, requestUrl);

            return await _gdax.Orders(sig, ts);
        }

        public async Task<IEnumerable<Fill>> GetFillsForOrder(string orderId)
        {
            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = string.Format("/fills?order_id={0}", orderId);
            string sig = ComputeSignature(ts, httpMethod, requestUrl);

            return await _gdax.FillsForOrder(sig, ts, orderId);
        }
        #endregion

        #region Private Helpers
        private string ComputeSignature(double timestamp, string httpMethod, string requestUrl, string requestBody = "")
        {
            byte[] data = Convert.FromBase64String(_config.Secret);
            var prehash = timestamp + httpMethod.ToUpper() + requestUrl + requestBody;
            return HashString(prehash, data);
        }

        private string HashString(string str, byte[] secret)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using (var hmac = new HMACSHA256(secret))
            {
                byte[] hash = hmac.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private static string GetProductIdFromSymbol(string symbol)
        {
            if (symbol.EndsWith("USD"))
            {
                return string.Format("{0}-USD", symbol.Replace("USD", ""));
            }
            else if (symbol.EndsWith("BTC"))
            {
                return string.Format("{0}-BTC", symbol.Replace("BTC", ""));
            }
            else if (symbol.EndsWith("ETH"))
            {
                return string.Format("{0}-ETH", symbol.Replace("ETH", ""));
            }
            else
            {
                return null;
            }
        }

        public Task<IAcceptedAction> MarketBuy(string generatedId, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> MarketSell(string generatedId, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
