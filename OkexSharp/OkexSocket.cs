using Core;
using Core.Config;
using Core.Contracts;
using Core.Models;
using Newtonsoft.Json;
using NLog;
using OkexSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace OkexSharp
{
    public class OkexSocket : ISocketExchange
    {
        protected SslProtocols SupportedProtocols { get; } = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

        WebSocket socket;

        IExchange _okex;
        ExchangeConfig _config;
        OrderBookCache cache;

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
                return _okex;
            }
        }

        public event Action<OrderBook> OnBook;
        public event Action<IMatch> OnMatch;

        NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public OkexSocket(ExchangeConfig config)
        {
            _okex = new Okex(config);
            _config = config;
            name = config.Name;
        }

        public void InitWebSocket()
        {
            this.disposed = false;

            socket = new WebSocket("wss://real.okex.com:10441/websocket");

            socket.SslConfiguration.EnabledSslProtocols = SupportedProtocols;

            socket.OnOpen += (sender, e) =>
                logger.Trace("OKEx WebSocket Connected");

            socket.OnError += (sender, e) =>
                logger.Trace(JsonConvert.SerializeObject(e, Formatting.Indented));

            socket.OnClose += (sender, e) =>
                logger.Trace("OKEx WebSocket Disconnected");
        }

        public async Task CacheOrderbook(string symbol)
        {
            var depth = await _okex.OrderBook(symbol);

            cache = new OrderBookCache(depth);
        }

        public void SubscribeOrderbook(string symbol)
        {
            InitWebSocket();

            CacheOrderbook(symbol).Wait();

            socket.OnMessage += async (sender, e) =>
            {
                try
                {
                    var payload = GetResponseData<OrderBookResponse>(e);

                    if (payload != null && payload.timestamp.HasValue)
                    {
                        const decimal defaultIgnoreValue = 0.00000000M;

                        if (payload.bids != null)
                        {
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
                        }

                        if (payload.asks != null)
                        {
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
                        }
                        
                        var book = cache.ToOrderBook();

                        book.asks = Helper.SumOrderEntries(book.asks);
                        book.bids = Helper.SumOrderEntries(book.bids);

                        DispatchBook(book);

                    }
                }
                catch (Exception ex)
                {
                    logger.Trace(JsonConvert.SerializeObject(ex, Formatting.Indented));
                }
                finally
                {
                    await Task.FromResult(true);
                }
            };

            socket.Connect();

            var request = new SocketRequest()
            {
                Event = "addChannel",
                Channel = string.Format("ok_sub_spot_{0}_depth", Okex.GetMarketNameFromSymbol(symbol))
            };

            socket.Send(JsonConvert.SerializeObject(request));
        }

        public void SubscribeTrades(string symbol)
        {
            throw new NotImplementedException();
        }

        private static T GetResponseData<T>(MessageEventArgs e)
        {
            return JsonConvert.DeserializeObject<List<SocketResponse<T>>>(e.Data).FirstOrDefault().Payload;
        }

        internal void DispatchBook(OrderBook book)
        {
            OnBook?.Invoke(book);
        }

        internal void DispatchTrade(Match trade)
        {
            OnMatch?.Invoke(trade);
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

            if (disposing && socket != null)
            {
                socket.Close();
                socket = null;
            }

            disposed = true;
        }

        public Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
