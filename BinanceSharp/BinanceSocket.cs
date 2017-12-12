using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using NLog;
using Core.Config;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Security.Authentication;
using Newtonsoft.Json;
using BinanceSharp.Models;
using System.Linq;
using Core;
using RestEase;
using Core.Engine;

namespace BinanceSharp
{
    public class BinanceSocket : ISocketExchange
    {

        protected SslProtocols SupportedProtocols { get; } = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

        Binance _binance;
        OrderBookCache cache;
        ExchangeConfig _config;
        WebSocket socket;

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public IExchange Rest
        {
            get
            {
                return _binance;
            }
        }

        public event Action<OrderBook> OnBook;
        public event Action<IMatch> OnMatch;

        NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public BinanceSocket(ExchangeConfig config)
        {
            _binance = new Binance(config);
            _config = config;
            name = config.Name;
        }

        public void InitWebSocket(string uri)
        {
            disposed = false;

            socket = new WebSocket(uri);

            socket.SslConfiguration.EnabledSslProtocols = SupportedProtocols;

            socket.OnOpen += (sender, e) =>
                Console.WriteLine("Binance WebSocket Connected");

            socket.OnError += (sender, e) =>          
                Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));           

            socket.OnClose += (sender, e) =>            
                Console.WriteLine("Binance WebSocket Disconnected");           
        }

        public async Task CacheOrderbook(string symbol)
        {
            try
            {
                var depth = await _binance.OrderBook(symbol);

                cache = new OrderBookCache(depth);
            }
            catch(ApiException ae)
            {
                logger.Error(ae);
                cache = new OrderBookCache(new OrderBook(symbol));
            }

        }

        public void SubscribeOrderbook(string symbol)
        {
            CacheOrderbook(symbol).Wait();

            InitWebSocket(string.Format("wss://stream.binance.com:9443/ws/{0}@depth", symbol.ToLowerInvariant()));

            socket.OnMessage += async (sender, e) =>
            {

                var payload = JsonConvert.DeserializeObject<OrderBookSocketResponse>(e.Data);

                const decimal defaultIgnoreValue = 0.00000000M;

                payload.bids.ForEach(b =>
                {
                    decimal price = Convert.ToDecimal(b[0]);
                    decimal size = Convert.ToDecimal(b[1]);

                    if (cache.Bids.ContainsKey(price))
                    {
                        if (size == defaultIgnoreValue)
                        {
                            OrderBookEntry o;
                            cache.Bids.Remove(price, out o);
                        }
                        else
                        {
                            cache.Bids[price] = new OrderBookEntry() { price = price, quantity = size };
                        }
                    }
                    else
                    {
                        if (size != defaultIgnoreValue)
                        {
                            cache.Bids[price] = new OrderBookEntry() { price = price, quantity = size };
                        }
                    }
                });

                payload.asks.ForEach(b =>
                {
                    decimal price = Convert.ToDecimal(b[0]);
                    decimal size = Convert.ToDecimal(b[1]);

                    if (cache.Asks.ContainsKey(price))
                    {
                        if (size == defaultIgnoreValue)
                        {
                            OrderBookEntry o;
                            cache.Asks.Remove(price, out o);
                        }
                        else
                        {
                            cache.Asks[price] = new OrderBookEntry() { price = price, quantity = size };
                        }
                    }
                    else
                    {
                        if (size != defaultIgnoreValue)
                        {
                            cache.Asks[price] = new OrderBookEntry() { price = price, quantity = size };
                        }
                    }
                });

                // Console.WriteLine(JsonConvert.SerializeObject(cache, Formatting.Indented));

                var book = cache.ToOrderBook();

                book.asks = Helper.SumOrderEntries(book.asks);
                book.bids = Helper.SumOrderEntries(book.bids);
                book.updateId = payload.updateId;

                DispatchBook(book);

                await Task.FromResult(true);

            };

            socket.Connect();
        }

        internal void DispatchBook(OrderBook book)
        {
            OnBook?.Invoke(book);
        }

        public void SubscribeTrades(string symbol)
        {
            var result = _binance.GetUserDataStream().Result;

            InitWebSocket(string.Format("wss://stream.binance.com:9443/ws/{0}", result.listenKey));

            socket.OnMessage += async (sender, e) =>
            {
                var userEvent = JsonConvert.DeserializeObject<UserDataResponse>(e.Data);

                if(userEvent.EventType == "executionReport" && userEvent.QuantityOfLastFilledTrade > 0 && symbol == userEvent.Symbol)
                {
                    //logger.Debug(JsonConvert.SerializeObject(userEvent, Formatting.Indented));

                    var match = new Match()
                    {
                        QuantityFilled = userEvent.QuantityOfLastFilledTrade,
                        Side = userEvent.Side.ToLowerInvariant(),
                        Symbol = userEvent.Symbol,
                        Uuid = userEvent.TradeId.ToString()
                    };
                    Console.WriteLine(JsonConvert.SerializeObject(match, Formatting.Indented));

                    DispatchTrade(match);
                }

                await Task.FromResult(true);
            };

            socket.Connect();
        }

        internal void DispatchTrade(Match match)
        {
            OnMatch?.Invoke(match);
        }

        public Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        #region IDisposable

        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Reset();
            }

            disposed = true;
        }

        public void Reset()
        {
            if(socket != null)
            {
                socket.Close();
                socket = null;
            }
        }

        #endregion
    }
}
