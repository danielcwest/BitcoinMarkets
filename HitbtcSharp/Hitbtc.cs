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
            _hitbtc = RestClient.For<IHitbtcApi>("https://api.hitbtc.com");
            _config = config;
            name = config.Name;
            fee = config.Fee;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var symbols = await _hitbtc.GetSymbols();
            return symbols.Select(s => new Symbol(s));
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _hitbtc.GetTickers();
            return summaries.Where(s => s.symbol.EndsWith("BTC") || s.symbol.EndsWith("ETH")).Select(s => new Market(s.symbol, s));
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

            book.asks = depth.ask.Select(e => new OrderBookEntry() { price = e.price, quantity = e.size }).Take(25).ToList();
            book.bids = depth.bid.Select(e => new OrderBookEntry() { price = e.price, quantity = e.size }).Take(25).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<IAcceptedAction> Buy(string generatedId, string symbol, decimal quantity, decimal rate)
        {
            long nonce = GetNonce();

            string pathAndQuery = string.Format("/api/1/trading/new_order?nonce={0}&apikey={1}clientOrderId={2}&symbol={3}&side=buy&price={4}&quantity={5}&type=limit", nonce, _config.ApiKey, nonce, symbol, rate, quantity);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var data = new Dictionary<string, object> {
                {"clientOrderId", generatedId},
                {"symbol", symbol },
                {"side", "buy"},
                {"price", rate},
                {"quantity", quantity},
                {"type", "limit" }
            };

            var order = await _hitbtc.PlaceOrder(sign, nonce, _config.ApiKey, data);

            return order;
        }

        public async Task<IAcceptedAction> Sell(string generatedId, string symbol, decimal quantity, decimal rate)
        {
            long nonce = GetNonce();

            string pathAndQuery = string.Format("/api/1/trading/new_order?nonce={0}&apikey={1}clientOrderId={2}&symbol={3}&side=sell&price={4}&quantity={5}&type=limit", nonce, _config.ApiKey, nonce, symbol, rate, quantity);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var data = new Dictionary<string, object> {
                {"clientOrderId", generatedId},
                {"symbol", symbol },
                {"side", "sell"},
                {"price", rate},
                {"quantity", quantity},
                {"type", "limit" }
            };

            var order = await _hitbtc.PlaceOrder(sign, nonce, _config.ApiKey, data);

            return order;
        }

        public async Task CancelOrder(string orderId)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/cancel_order?nonce={0}&apikey={1}&clientOrderId={2}", nonce, _config.ApiKey, orderId);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            await _hitbtc.CancelOrder(sign, nonce, _config.ApiKey, orderId);
        }
        public async Task<IOrder> CheckOrder(string orderId)
        {
            int limit = 25;
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/orders/recent?nonce={0}&apikey={1}&max_results={2}", nonce, _config.ApiKey, limit);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var orders = await _hitbtc.GetOrders(sign, nonce, _config.ApiKey, limit);
            var order = orders.orders.Find(o => o.Uuid == orderId);

            return new Order(order);
        }

        //Hitbtc requires a transfer from trading to main before we can withdrawal
        public async Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            var balance = await GetMainBalance(currency);

            CryptoTransaction tx;
            if (quantity > balance.Balance)
                tx = await TransferToMain(quantity, currency);

            return await WithdrawFromMain(currency, quantity, address, paymentId);
        }

        private async Task<IAcceptedAction> WithdrawFromMain(string currency, decimal quantity, string address, string paymentId = null)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/payout?nonce={0}&apikey={1}amount={2}&currency_code={3}&address={4}", nonce, _config.ApiKey, quantity, currency, address);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var data = new Dictionary<string, object> {
                {"amount", quantity},
                {"currency_code", currency },
                {"address", address}
            };

            return await _hitbtc.Withdraw(sign, nonce, _config.ApiKey, data);
        }

        public async Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/transactions/{0}?nonce={1}&apikey={2}", uuid, nonce, _config.ApiKey);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);
            var tx = await _hitbtc.GetWithdrawal(sign, nonce, _config.ApiKey, uuid);
            return new Withdrawal(tx.transaction);
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
            var balances = await GetBalances("trading", false);

            return balances.Where(b => b.Currency == currency).FirstOrDefault();
        }

        public async Task<ICurrencyBalance> GetMainBalance(string currency)
        {
            var balances = await GetBalances("payment", false);

            return balances.Where(b => b.Currency == currency).FirstOrDefault();
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

        public async Task<CryptoTransaction> TransferToTrading(decimal amount, string currency)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/transfer_to_trading?nonce={0}&apikey={1}amount={2}&currency_code={3}", nonce, _config.ApiKey, amount, currency);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var data = new Dictionary<string, object> {
                {"amount", amount},
                {"currency_code", currency }
            };

            return await _hitbtc.TransferToTrading(sign, nonce, _config.ApiKey, data);
        }

        public async Task<CryptoTransaction> TransferToMain(decimal amount, string currency)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/transfer_to_main?nonce={0}&apikey={1}amount={2}&currency_code={3}", nonce, _config.ApiKey, amount, currency);

            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var data = new Dictionary<string, object> {
                {"amount", amount},
                {"currency_code", currency }
            };

            return await _hitbtc.TransferToMain(sign, nonce, _config.ApiKey, data);
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
