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

namespace HitbtcSharp
{
    public class HitbtcSocket : ISocketExchange
    {
        IExchange _hitbtc;
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

        public IExchange Rest
        {
            get
            {
                return _hitbtc;
            }
        }

        public event Action<OrderBook> OnBook;
        public event Action<IMatch> OnMatch;

        NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public HitbtcSocket(ExchangeConfig config)
        {
            _hitbtc = new Hitbtc(config);
            _config = config;
            name = config.Name;
        }

        public void InitWebSocket()
        {
            disposed = false;

            client = new RpcClient("wss://api.hitbtc.com/api/2/ws");

            client.On<HitbtcOrderV2, bool>("report", (payload) =>
            {
               // Console.WriteLine(JsonConvert.SerializeObject(payload, Formatting.Indented));
                DispatchTrade(new Match(payload));
                return Task.FromResult(true);
            });

            client.On<object, bool>("activeOrders", (payload) =>
            {
                return Task.FromResult(true);
            });

            client.On<RpcOrderbookResponse, bool>("snapshotOrderbook", (payload) =>
            {
                cache = new OrderBookCache(new Core.Models.OrderBook(payload.symbol)
                {
                    asks = payload.ask.Select(e => new OrderBookEntry() { price = e.price, quantity = e.size }).Take(10).ToList(),
                    bids = payload.bid.Select(e => new OrderBookEntry() { price = e.price, quantity = e.size }).Take(10).ToList(),
                    updateId = payload.sequence
                });
                return Task.FromResult(true);
            });

            long lastUpdateId = 0;

            client.On<RpcOrderbookResponse, bool>("updateOrderbook", (payload) =>
            {
                const decimal defaultIgnoreValue = 0.00000000M;

                if (payload.sequence > lastUpdateId && cache != null)
                {
                    payload.bid.ForEach(b =>
                    {
                        if (cache.Bids.ContainsKey(b.price))
                        {
                            if (b.size == defaultIgnoreValue)
                            {
                                OrderBookEntry o;
                                cache.Bids.Remove(b.price, out o);
                            }
                            else
                            {
                                cache.Bids[b.price] = new OrderBookEntry() { price = b.price, quantity = b.size };
                            }
                        }
                        else
                        {
                            if (b.size != defaultIgnoreValue)
                            {
                                 cache.Bids[b.price] = new OrderBookEntry() { price = b.price, quantity = b.size };
                            }
                        }
                    });

                    payload.ask.ForEach(b =>
                    {
                        if (cache.Asks.ContainsKey(b.price))
                        {
                            if (b.size == defaultIgnoreValue)
                            {
                                OrderBookEntry o;
                                cache.Bids.Remove(b.price, out o);
                            }
                            else
                            {
                                cache.Asks[b.price] = new OrderBookEntry() { price = b.price, quantity = b.size };
                            }
                        }
                        else
                        {
                            if (b.size != defaultIgnoreValue)
                            {
                                cache.Asks[b.price] = new OrderBookEntry() { price = b.price, quantity = b.size };
                            }
                        }
                    });

                    if(payload.sequence % 5 == 0)
                    {
                        var book = cache.ToOrderBook();
                        book.updateId = payload.sequence;
                        book.bids = Helper.SumOrderEntries(book.bids);
                        book.asks = Helper.SumOrderEntries(book.asks);

                        DispatchBook(book);
                    }
                    


                    lastUpdateId = payload.sequence;
                }

                return Task.FromResult(true);
            });

            client.Error += (e) =>            
                Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));            

            client.Disconnected += () =>           
                Console.WriteLine("Hitbtc WebSocket Disconnected");            
        }

        public void SubscribeOrderbook(string symbol)
        {
            InitWebSocket();

            client.Connected += () =>
            {
                Console.WriteLine("Hitbtc WebSocket Connected");

                try
                {
                    var result = client.Call<object, bool>("subscribeOrderbook", new { symbol = symbol }).Result;
                    if (result)
                        Console.WriteLine("Hitbtc Subscribed to Orderbook feed");
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            };
        }

        internal void DispatchBook(OrderBook book)
        {
            OnBook?.Invoke(book);
        }

        internal void DispatchTrade(Match trade)
        {
            OnMatch?.Invoke(trade);
        }

        public void SubscribeTrades(string symbol)
        {
            InitWebSocket();

            client.Connected += () =>
            {
                Console.WriteLine("Hitbtc WebSocket Connected");
                try
                {
                    var loginResult = client.Call<object, bool>("login", new { algo = "BASIC", pKey = _config.ApiKey, sKey = _config.Secret }).Result;
                    if (loginResult)
                    {
                        var result = client.Call<object, bool>("subscribeReports", new { }).Result;
                        if (result)
                            Console.WriteLine("Hitbtc Subscribed to Trades");
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            };
        }

        public void Login()
        {
            InitWebSocket();

            client.Connected += () =>
            {
                try
                {
                    var result = client.Call<object, bool>("login", new { algo = "BASIC", pKey = _config.ApiKey, sKey = _config.Secret }).Result;
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            };
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

            if (disposing && client != null)
            {
                client.Dispose();
                client = null;
            }

            disposed = true;
        }

        public async Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            string clientId = Guid.NewGuid().ToString().Split("-").Last();

            var request = new OrderRequest()
            {
                clientOrderId = clientId,
                side = side,
                type = "limit",
                symbol = symbol,
                price = price,
                quantity = quantity,
                timeInForce = "IOC"
            };

            await client.Call<OrderRequest, object>("newOrder", request);
            return new HitbtcOrder() { Uuid = clientId };
        }

        #endregion
    }
}
