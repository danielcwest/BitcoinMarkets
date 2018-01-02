using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using Core.Config;
using HitbtcSharp.RPC;
using System.Threading.Tasks;
using Core;
using System.Linq;
using HitbtcSharp.Models;
using RestEase;
using NLog;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using Core.Handlers;
using System.Security.Authentication;
using System.Threading;

namespace HitbtcSharp
{
    public class HitbtcSocket : IBaseSocketExchange
    {
        protected SslProtocols SupportedProtocols { get; } = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

        IExchange _hitbtc;
        ExchangeConfig _config;
        OrderBookCache cache;

        WebSocket socket;

        public TimeSpan CallTimeOut { get; set; }


        private ConcurrentDictionary<string, SocketTradeHandler> _handlers;
        private ConcurrentDictionary<string, TaskCompletionSource<JToken>> _requests;
        private ConcurrentDictionary<string, ArbitragePair> _pairs;

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
                return _hitbtc;
            }
        }

        NLog.Logger logger = LogManager.GetCurrentClassLogger();

        CancellationToken cancellationToken;

        public HitbtcSocket(ExchangeConfig config)
        {
            _hitbtc = new Hitbtc(config);
            _config = config;
            name = config.Name;
            CallTimeOut = TimeSpan.FromSeconds(10);

            _handlers = new ConcurrentDictionary<string, SocketTradeHandler>();
            _requests = new ConcurrentDictionary<string, TaskCompletionSource<JToken>>();
            _pairs = new ConcurrentDictionary<string, ArbitragePair>();
        }

        public void RegisterPair(ArbitragePair pair, SocketTradeHandler handler)
        {
            if (!_pairs.TryAdd(pair.Symbol, pair)) throw new Exception("Duplicate pair");
            if (!_handlers.TryAdd(pair.Symbol, handler)) throw new Exception("Duplicate handler");
        }

        public void InitWebSocket()
        {
            disposed = false;

            socket = new WebSocket(Domain.BASE_SOCKET_URL);

            socket.SslConfiguration.EnabledSslProtocols = SupportedProtocols;

            socket.OnOpen += (sender, e) =>           
                Console.WriteLine("Hitbtc WebSocket Connected");
            

            socket.OnError += (sender, e) =>
                Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));

            socket.OnClose += (sender, e) =>
                Console.WriteLine("Binance WebSocket Disconnected");

            socket.OnMessage += async (sender, e) =>
            {
                var message = RpcMessage.Read(e.Data);
                await ProcessMessage(message);
            };

            socket.Connect();
        }

        public void SubscribeTrades(CancellationToken token)
        {
            cancellationToken = token;
            InitWebSocket();

            var result = Login().Result.ToObject<bool>();

            if (result)
            {
                var payload = JToken.FromObject(new { });
                Invoke("subscribeReports", payload).Wait();
            }
        }



        public async Task<JToken> Login()
        {
            var payload = JToken.FromObject(new { algo = "BASIC", pKey = _config.ApiKey, sKey = _config.Secret });
            return await Invoke("login", payload);
        }

        internal async Task<JToken> Invoke(string method, JToken payload)
        {
            var id = Guid.NewGuid().ToString();

            try
            {
                
                var completion = new TaskCompletionSource<JToken>();

                if (!_requests.TryAdd(id, completion))
                    throw new Exception("Duplicate request id");

                var req = new RpcRequest
                {
                    Id = id,
                    Method = method,
                    Parameters = payload
                };

                string json = JsonConvert.SerializeObject(req);

                if (socket != null && socket.ReadyState == WebSocketState.Open)
                {
                    socket.Send(json);
                }

                var timeout = Task.Delay(CallTimeOut);
                var completed = await Task.WhenAny(completion.Task, timeout);

                if (completed == timeout)
                {
                    //logger.Error("{0} Request timed out", method);
                   // throw new Exception("Timeout");
                }

                if (method == "login")
                    Console.WriteLine("Hitbtc Authenticated");

                return completion.Task.Result;
            }
            finally
            {
                TaskCompletionSource<JToken> removed;
                _requests.TryRemove(id, out removed);
            }

        }

        internal async Task ProcessMessage(IRpcMessage message)
        {
            try
            {
                switch (message.Type)
                {
                    case RpcMessageType.Notification:
                        var notification = (RpcNotification)message;

                        if (string.IsNullOrWhiteSpace(notification.Method))
                            break;

                        if (notification.Method == "report")
                        {
                            var trade = notification.Payload.ToObject<HitbtcOrderV2>();

                            ArbitragePair pair;
                            if (_pairs.TryGetValue(trade.symbol, out pair))
                            {
                                SocketTradeHandler notificationHandler;
                                if (_handlers.TryGetValue(trade.symbol, out notificationHandler))
                                {
                                    await notificationHandler(this, pair, new Match(trade));
                                }
                            }
                        }
                        break;
                    case RpcMessageType.Request:
                        throw new Exception("not handling requests");

                    case RpcMessageType.Response:
                        var response = (RpcResponse)message;

                        TaskCompletionSource<JToken> completion;
                        if (!_requests.TryGetValue(response.Id, out completion))
                            throw new Exception("Unknown request id");

                        if (response.Error != null)
                        {
                            completion.SetException(new RpcException(response.Error));
                            return;
                        }

                        completion.SetResult(response.Result);
                        break;

                    default:
                        throw new NotSupportedException(string.Format("Unsupported message type '{0}'", message.Type));
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        public async Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            string clientId = Guid.NewGuid().ToString().Split("-").Last();

            var order = new OrderRequest()
            {
                clientOrderId = clientId,
                side = side,
                type = "limit",
                symbol = symbol,
                price = price,
                quantity = quantity,
                timeInForce = "IOC"
            };

            var payload = JToken.FromObject(order);

            if (socket != null && socket.ReadyState == WebSocketState.Open)
            {
                await Invoke("newOrder", payload);
            }
            return new HitbtcOrder() { Uuid = clientId };
        }

        public void Reset()
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
            _pairs.Clear();
            _handlers.Clear();
            _requests.Clear();
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

        #endregion
    }
}