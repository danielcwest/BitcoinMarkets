using System;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using System.Threading.Tasks;
using HitbtcSharp.Models;
using Core.Contracts;
using Core;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using Core.Config;
using System.IO;
using Newtonsoft.Json;
using HitbtcSharp.RPC;
using Core.Engine;
using NLog;

namespace HitbtcSharp
{
    public class Hitbtc : IExchange
    {
        IHitbtcApi _hitbtc;
        ExchangeConfig _config;
        RpcClient client;
        OrderBookCache cache;

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public Hitbtc(ExchangeConfig config)
        {
            _hitbtc = RestClient.For<IHitbtcApi>("https://api.hitbtc.com");
            _config = config;
            name = config.Name;

            string auth = string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _config.ApiKey, _config.Secret))));
            _hitbtc.Authorization = auth;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var symbols = await _hitbtc.GetSymbols();
            return symbols.Select(s => new Symbol(s));
        }

        public async Task<IEnumerable<ITicker>> MarketSummaries()
        {
            var summaries = await _hitbtc.GetTickers();
            return summaries.Select(s => new Market(s.symbol, s));
        }

        public async Task<ITicker> Ticker(string symbol)
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

        public async Task<IAcceptedAction> MarketBuy(string symbol, decimal quantity)
        {

            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"side", "buy"},
                {"quantity", quantity.ToString("#.#####")},
                {"type", "market" },
                {"timeInForce", "IOC" }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public async Task<IAcceptedAction> MarketSell(string symbol, decimal quantity)
        {
            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"side", "sell"},
                {"quantity", quantity.ToString("#.#####")},
                {"type", "market" },
                {"timeInForce", "IOC" }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public async Task<IAcceptedAction> LimitBuy(string symbol, decimal quantity, decimal price)
        {
            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"side", "buy"},
                {"price", price},
                {"quantity", quantity.ToString("#.#")},
                {"type", "limit" },
                {"timeInForce", "GTD" },
                {"expireTime", DateTime.UtcNow.AddSeconds(30) }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }



        public async Task<IAcceptedAction> LimitSell(string symbol, decimal quantity, decimal price)
        {
            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"side", "sell"},
                {"price", price},
                {"quantity", quantity.ToString("#.#")},
                {"type", "limit" },
                {"timeInForce", "GTD" },
                {"expireTime", DateTime.UtcNow.AddSeconds(30) }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public async Task CancelOrder(string orderId)
        {
            await _hitbtc.CancelOrder(orderId);
        }

        public async Task<IOrder> CheckOrder(string orderId, string symbol = "")
        {
            string dateStr = DateTime.UtcNow.AddHours(-1).ToString("o");
            var orders = await _hitbtc.GetOrdersByDate(dateStr);
            var order = orders.Where(o => o.Uuid == orderId).FirstOrDefault();

            if(order != null)
            {
                var trades = await GetTradesForOrder(order.symbol, order.Uuid);

                return new Order(order, trades);
            }
            else
            {
                return null;
            }

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
            await MoveBalancesToTrading();
            return await _hitbtc.GetTradingBalances();
        }

        private async Task MoveBalancesToTrading()
        {
            try
            {
                foreach (var b in GetBalances("main").Result)
                {
                    try
                    {
                        if (b.Available < 0.09m) continue;

                        var result = await TransferToTrading(b.Available, b.Currency);
                    }
                    catch (Exception e)
                    {
                        //Fail silently for now
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                //Fail silently for now
                Console.WriteLine(e);
            }
        }

        public async Task<IEnumerable<Trade>> GetTradesForOrder(string symbol, string orderId)
        {
            var trades = await _hitbtc.GetTrades(symbol);
            return trades.Where(t => t.orderId == orderId);
        }

        public async Task<IEnumerable<string>> CancelOrders(string symbol)
        {
           var orders = await _hitbtc.CancelOrders(symbol);
            return orders.Select(o => o.Uuid);
        }

        public async Task<IAcceptedAction> FillOrKill(string side, string symbol, decimal quantity, decimal price)
        {
            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"side", side},
                {"price", price},
                {"quantity", quantity.ToString("#.####")},
                {"type", "limit" },
                {"timeInForce", "FOK" }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public async Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"side", side},
                {"price", price},
                {"quantity", quantity.ToString("#.####")},
                {"type", "limit" },
                {"timeInForce", "IOC" }
            };

            var order = await _hitbtc.PlaceOrder(data);

            return order;
        }

        public ISocketExchange GetSocket()
        {
            return new HitbtcSocket(_config);
        }
    }
}
