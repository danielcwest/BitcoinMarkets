using System;
using System.Threading.Tasks;
using BinanceSharp.Models;
using System.Collections.Generic;
using RestEase;
using Core.Models;
using System.Linq;
using Core.Contracts;
using Core;
using Core.Config;
using System.Text;
using System.Security.Cryptography;

namespace BinanceSharp
{

    public class Binance : IExchange
    {
        IBinanceApi _binance;
        ExchangeConfig _config;

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Binance(ExchangeConfig config)
        {
            _binance = RestClient.For<IBinanceApi>("https://api.binance.com");
            _config = config;
            name = config.Name;
        }

        public async Task<IEnumerable<ISymbol>> Symbols()
        {
            var prices = await _binance.GetPrices();
            return prices.Select(p => new Symbol(p));
        }

        public async Task<IEnumerable<ITicker>> MarketSummaries()
        {
            var prices = await _binance.GetPrices();

            var pricesDic = prices.ToDictionary(p => p.symbol);

            var books = await _binance.GetBooks();

            var tickersDic = books.ToDictionary(b => b.symbol);

            return pricesDic.Keys.Select(k => new Market(pricesDic[k], tickersDic[k]));
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);
            var depth = await _binance.GetOrderBook(symbol);

            book.asks = depth.asks.Select((a, index) => new OrderBookEntry() { price = Convert.ToDecimal(a[0]), quantity = Convert.ToDecimal(a[1]) }).Take(25).ToList();
            book.bids = depth.bids.Select((a, index) => new OrderBookEntry() { price = Convert.ToDecimal(a[0]), quantity = Convert.ToDecimal(a[1]) }).Take(25).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public async Task<ITicker> Ticker(string symbol)
        {
            var ticker = await _binance.GetTicker(symbol);
            return new Market(symbol, ticker);
        }

        public async Task<ListenKey> GetUserDataStream()
        {
            return await _binance.UserDataStream(_config.ApiKey);
        }

        public Task CancelOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<IOrder> CheckOrder(string uuid, string symbol)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("symbol={0}&orderId={1}&timestamp={2}", symbol, uuid, timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"orderId", uuid },
                {"timestamp", timestamp},
                {"signature", sign}
            };


            var order = await _binance.GetOrder(_config.ApiKey, data);

            var trades = await GetTrades(symbol, order.time);

            ITicker bnbTicker = null;
            if (symbol.Contains("BTC"))
            {
                bnbTicker = await Ticker("BNBBTC");
            }else if (symbol.Contains("ETH"))
            {
                bnbTicker = await Ticker("BNBETH");
            }

            return new Order(order, trades.Where(t => t.time == order.time), bnbTicker);
        }

        public async Task<IEnumerable<BinanceTrade>> GetTrades(string symbol)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("symbol={0}&timestamp={1}", symbol, timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"timestamp", timestamp},
                {"signature", sign}
            };

            return await _binance.GetTrades(_config.ApiKey, data);
        }

        public async Task<IEnumerable<BinanceTrade>> GetTrades(string symbol, long ts)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("symbol={0}&timestamp={1}", symbol, timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"timestamp", timestamp},
                {"signature", sign}
            };

            var trades = await GetPagedTrades(symbol, ts, 0);

            while(trades.LastOrDefault().time < ts)
            {
                trades = await GetPagedTrades(symbol, ts, trades.LastOrDefault().id);
            }

            return trades;
        }

        public async Task<IEnumerable<BinanceTrade>> GetPagedTrades(string symbol, long ts, int fromId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("symbol={0}&fromId={1}&timestamp={2}", symbol, fromId, timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"symbol", symbol },
                {"fromId", fromId },
                {"timestamp", timestamp},
                {"signature", sign}
            };

            var trades = await _binance.GetTrades(_config.ApiKey, data);

            return trades;
        }

        public async Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
            {
                return await WithdrawNoTag(currency, quantity, address);
            }
            else
            {
                return await WithdrawWithTag(currency, quantity, address, paymentId);
            }
        }

        public async Task<IAcceptedAction> WithdrawNoTag(string currency, decimal quantity, string address)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("asset={0}&address={1}&amount={2}&name={3}&timestamp={4}", currency, address, quantity, "hitbtc", timestamp);

            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"asset", currency },
                {"address", address },
                {"amount", quantity },
                {"name", "hitbtc" },
                {"timestamp", timestamp},
                {"signature", sign}
            };

            return await _binance.Withdraw(_config.ApiKey, data);
        }

        public async Task<IAcceptedAction> WithdrawWithTag(string currency, decimal quantity, string address, string tag)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("asset={0}&address={1}&addressTag={2}&amount={3}&name={4}&timestamp={5}", currency, address, tag, quantity, "hitbtc", timestamp);

            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"asset", currency },
                {"address", address },
                {"addressTag", tag },
                {"amount", quantity },
                {"name", "hitbtc" },
                {"timestamp", timestamp},
                {"signature", sign}
            };

            return await _binance.Withdraw(_config.ApiKey, data);
        }

        public async Task<IDepositAddress> GetDepositAddress(string currency)
        {
            int recvWindow = 1000000;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("asset={0}&recvWindow={1}&timestamp={2}", currency, recvWindow,  timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"asset", currency },
                {"recvWindow", recvWindow  },
                {"timestamp", timestamp},
                {"signature", sign}
            };

           var resp = await _binance.DepositAddress(_config.ApiKey, data);
            return new DepositAddress(resp);
        }

        public async Task<IAcceptedAction> LimitBuy(string symbol, decimal quantity, decimal rate)
        {
            return await NewLimitOrder("buy", symbol, quantity, rate, "GTC");
        }

        public async Task<IAcceptedAction> LimitSell(string symbol, decimal quantity, decimal rate)
        {
            return await NewLimitOrder("sell", symbol, quantity, rate, "GTC");
        }

        public async Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("timestamp={0}", timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"timestamp", timestamp},
                {"signature", sign}
            };

            var withdrawals = await _binance.WithdrawHistory(_config.ApiKey, data);

            return withdrawals.Single(w => w.Uuid == uuid);
        }

        public async Task<IEnumerable<ICurrencyBalance>> GetBalances()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string totalParams = string.Format("timestamp={0}", timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"timestamp", timestamp},
                {"signature", sign}
            };

            var account = await _binance.GetAccount(_config.ApiKey, data);

            return account.balances.Select(b => new Balance(b));
        }

        Task<IEnumerable<string>> IExchange.CancelOrders(string symbol)
        {
            throw new NotImplementedException();
        }

        public async Task<IAcceptedAction> MarketBuy(string symbol, decimal quantity)
        {
            return await NewMarketOrder("buy", symbol, quantity);
        }

        public async Task<IAcceptedAction> MarketSell(string symbol, decimal quantity)
        {
            return await NewMarketOrder("sell", symbol, quantity);
        }

        public Task<IAcceptedAction> FillOrKill(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public async Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            return await NewLimitOrder(side, symbol, quantity, price, "IOC");
        }

        private async Task<OrderResponse> NewMarketOrder(string side, string symbol, decimal quantity)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            string totalParams = string.Format("symbol={0}&side={1}&type={2}&quantity={3}&timestamp={4}", symbol, side, "MARKET", quantity, timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"symbol", symbol},
                {"side", side },
                {"type", "MARKET"},
                {"quantity", quantity },
                {"timestamp", timestamp},
                {"signature", sign}
            };

            return await _binance.NewOrder(_config.ApiKey, data);
        }

        private async Task<OrderResponse> NewLimitOrder(string side, string symbol, decimal quantity, decimal price, string timeInForce)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            string totalParams = string.Format("symbol={0}&side={1}&type={2}&timeInForce={3}&quantity={4}&price={5}&timestamp={6}", symbol, side, "LIMIT", timeInForce, quantity, price, timestamp);
            string sign = CalculateSignature(totalParams, _config.Secret);

            var data = new Dictionary<string, object> {
                {"symbol", symbol},
                {"side", side },
                {"type", "LIMIT"},
                {"timeInForce", timeInForce},
                {"quantity", quantity },
                {"price", price},
                {"timestamp", timestamp},
                {"signature", sign}
            };

            return await _binance.NewOrder(_config.ApiKey, data);
        }

        private static string CalculateSignature(string text, string secretKey)
        {
            using (var hmacsha512 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(text));
                return string.Concat(hmacsha512.Hash.Select(b => b.ToString("x2")).ToArray()); // minimalistic hex-encoding and lower case
            }
        }

        public ISocketExchange GetSocket()
        {
            return new BinanceSocket(_config);
        }
    }

}