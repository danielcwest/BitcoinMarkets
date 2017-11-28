using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using Core.Config;
using NLog;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Security.Authentication;
using GdaxSharp.Models;
using System.Security.Cryptography;
using Core.Util;

namespace GdaxSharp
{
    public class GdaxSocket : ISocketExchange
    {

        protected SslProtocols SupportedProtocols { get; } = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

        WebSocket socket;

        IExchange _gdax;
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
                return _gdax;
            }
        }

        public event Action<OrderBook> OnBook;
        public event Action<IMatch> OnMatch;

        NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public GdaxSocket(ExchangeConfig config)
        {
            _gdax = new Gdax(config);
            _config = config;
            name = config.Name;
        }

        public void InitWebSocket()
        {
            disposed = false;

            socket = new WebSocket("wss://ws-feed.gdax.com");

            socket.SslConfiguration.EnabledSslProtocols = SupportedProtocols;

            socket.OnOpen += (sender, e) =>
                logger.Trace("Gdax WebSocket Connected");

           

            socket.OnError += (sender, e) =>
                logger.Trace(JsonConvert.SerializeObject(e, Formatting.Indented));

            socket.OnClose += (sender, e) =>
                logger.Trace("GDax WebSocket Disconnected");            
        }


        public void SubscribeOrderbook(string symbol)
        {
            InitWebSocket();

            socket.OnMessage += async (sender, e) =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        var response = JsonConvert.DeserializeObject<SocketResponse>(e.Data);

                        if (response.type == "l2update")
                        {
                            var update = JsonConvert.DeserializeObject<BookUpdateResponse>(e.Data);
                        }
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

            var request = CreateSocketRequest("level2", symbol);
            socket.Send(JsonConvert.SerializeObject(request));
            logger.Trace("Gdax Subscribed to Orderbook feed");

        }

        public void SubscribeTrades(string symbol)
        {
            InitWebSocket();

            socket.OnMessage += async (sender, e) =>
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {

                        var response = JsonConvert.DeserializeObject<SocketResponse>(e.Data);

                        if (response.type == "match")
                        {
                            var order = JsonConvert.DeserializeObject<SocketTrade>(e.Data);
                            DispatchTrade(new Match(order));
                        }

                        if (response.type == "received")
                        {
                            var order = JsonConvert.DeserializeObject<SocketOrder>(e.Data);
                        }
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

            var request = CreateSocketRequest("user", symbol);
            socket.Send(JsonConvert.SerializeObject(request));
            logger.Trace("Gdax Subscribed to Trades");

        }

        private SocketRequest CreateSocketRequest(string channel, string symbol)
        {
            var request = new SocketRequest();

            double ts = DateTime.UtcNow.ToUnixTimestamp();
            string httpMethod = "GET";
            string requestUrl = "/users/self/verify";
            string sig = ComputeSignature(ts, httpMethod, requestUrl);

            request.type = "subscribe";
            request.key = _config.ApiKey;
            request.passphrase = _config.Passphrase;
            request.timestamp = ts.ToString();
            request.signature = sig;
            request.channels = new List<string> { channel };
            request.product_ids = new List<string> { Gdax.GetProductIdFromSymbol(symbol) };
            return request;
        }

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

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        public Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
