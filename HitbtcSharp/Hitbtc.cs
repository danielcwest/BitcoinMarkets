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
using System.Net.Http.Headers;
using BMCore.Config;
using System.IO;
using Newtonsoft.Json;

namespace HitbtcSharp
{
    public class Hitbtc : IExchange
    {
        IHitbtcApi _hitbtc;
        ExchangeConfig _config;
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

        public Hitbtc(ExchangeConfig config)
        {
            _hitbtc = RestClient.For<IHitbtcApi>("https://api.hitbtc.com");
            _config = config;
            name = config.Name;
            fee = config.Fee;

            string auth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _config.ApiKey, _config.Secret))));
            _hitbtc.Authorization = auth;

            try
            {
                foreach (var b in GetBalances("main").Result)
                {
                    try
                    {
                        var result = TransferToTrading(b.Available, b.Currency).Result;
                    }
                    catch (Exception)
                    {
                        //Fail silently for now
                    }
                }
            }
            catch (Exception)
            {
                //Fail silently for now
            }


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

        public async Task<IAcceptedAction> Buy(string generatedId, string symbol, decimal quantity, decimal price)
        {

            var data = new Dictionary<string, object> {
                {"clientOrderId", generatedId},
                {"symbol", symbol },
                {"side", "buy"},
                {"price", price},
                {"quantity", quantity.ToString("#.####")},
                {"type", "limit" }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public async Task<IAcceptedAction> Sell(string generatedId, string symbol, decimal quantity, decimal price)
        {
            var data = new Dictionary<string, object> {
                {"clientOrderId", generatedId},
                {"symbol", symbol },
                {"side", "sell"},
                {"price", price},
                {"quantity", quantity.ToString("#.####")},
                {"type", "limit" }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public async Task CancelOrder(string orderId)
        {
            await _hitbtc.CancelOrder(orderId);
        }
        public async Task<IOrder> CheckOrder(string orderId)
        {
            var orders = await _hitbtc.GetOrders();
            var order = orders.Where(o => o.Uuid == orderId).FirstOrDefault();

            if (order == null) return null;

            var allTrades = await _hitbtc.GetTrades(order.symbol);
            var trades = allTrades.Where(t => t.orderId == orderId);

            return new Order(order, trades);
        }

        //Hitbtc requires a transfer from trading to main before we can withdrawal
        public async Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            var balance = await GetMainBalance(currency);

            PayoutTransaction tx;
            if (quantity > balance.Available)
                tx = await TransferToMain(quantity, currency);

            return await WithdrawFromMain(currency, quantity, address, paymentId);
        }

        private async Task<PayoutTransaction> WithdrawFromMain(string currency, decimal quantity, string address, string paymentId = null)
        {
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/payment/payout?nonce={0}&apikey={1}amount={2}&currency_code={3}&address={4}", nonce, _config.ApiKey, quantity, currency, address);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var data = new Dictionary<string, object> {
                {"amount", quantity},
                {"currency_code", currency },
                {"address", address}
            };

            return await _hitbtc.WithdrawV1(sign, nonce, _config.ApiKey, data);
        }

        public async Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            var tx = await _hitbtc.GetWithdrawal(uuid);

            //File.WriteAllText("hitbtc_withdrawal.json", JsonConvert.SerializeObject(tx));

            return new Withdrawal(tx);
        }

        public async Task<IDepositAddress> GetDepositAddress(string currency)
        {
            var address = await _hitbtc.GetAddress(currency);
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
            IEnumerable<ICurrencyBalance> balances;

            if (type == "trading")
                balances = await _hitbtc.GetTradingBalances();
            else
                balances = await _hitbtc.GetMainBalances();

            if (filterZero)
                return balances.Where(b => b.Available > 0);
            else
                return balances;
        }

        public async Task<PayoutTransaction> TransferToTrading(decimal amount, string currency)
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

        public async Task<PayoutTransaction> TransferToMain(decimal amount, string currency)
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

        public async Task<IEnumerable<ICurrencyBalance>> GetBalances()
        {
            return await _hitbtc.GetTradingBalances();
        }

        public async Task<HitbtcOrderV1> GetOrderV1(string orderId)
        {
            int limit = 25;
            long nonce = GetNonce();
            string pathAndQuery = string.Format("/api/1/trading/orders/recent?nonce={0}&apikey={1}&max_results={2}", nonce, _config.ApiKey, limit);
            string sign = CalculateSignature(pathAndQuery, _config.Secret);

            var orderRes = await _hitbtc.GetOrdersV1(sign, nonce, _config.ApiKey, limit);
            return orderRes.orders.Find(o => o.orderId == orderId);
        }

        public async Task<IEnumerable<Trade>> GetTradesForOrder(string symbol, string orderId)
        {
            var trades = await _hitbtc.GetTrades(symbol);
            return trades.Where(t => t.orderId == orderId);
        }

    }
}
