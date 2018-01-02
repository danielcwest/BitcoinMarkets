using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Core.Config;
using Core.Contracts;
using Core.DbService;
using NLog;
using Core.Models;
using System.Linq;
using RestEase;
using Newtonsoft.Json;
using Core.Domain;
using Core.Handlers;

namespace Core.Engine
{
    public class ArbitrageEngineV2
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        IExchange baseExchange;
        IExchange counterExchange;

        ConcurrentDictionary<string, ISocketExchange> counterSockets;

        IDbService dbService;

        //   ConcurrentDictionary<string, CancellationTokenSource> tokens;

     //   internal delegate Task<IMatch> HitbtcNotificationHandler(IBaseSocketExchange sender, IMatch payload);


        public ArbitrageEngineV2(IExchange baseExchange, IExchange counterExchange, IDbService dbService)
        {
            this.counterExchange = counterExchange;
            this.dbService = dbService;
            counterSockets = new ConcurrentDictionary<string, ISocketExchange>();
        }

        public void StartEngine(IBaseSocketExchange baseExchange)
        {
            Console.WriteLine("Starting Engine V2");

            var wtoken = new CancellationTokenSource();

            // Set the tasks.
            var task = (ActionBlock<IBaseSocketExchange>)CreateNeverEndingTask((e, ct) => DoWork(e, ct), wtoken.Token);

            task.Post(baseExchange);

            task.Completion.Wait();
        }

        public async Task DoWork(IBaseSocketExchange baseExchange, CancellationToken token)
        {
            var pairs = dbService.GetArbitragePairs("market", baseExchange.Name, counterExchange.Name).Select(p => new ArbitragePair(p));

            SocketTradeHandler handler = ProcessTrade;

            foreach(var pair in pairs)
            {
                baseExchange.RegisterPair(pair, handler);
            }
            baseExchange.SubscribeTrades(token);

            Parallel.ForEach(pairs, pair =>
            {
                try
                {
                    ProcessOrderBook(baseExchange, pair, token).Wait();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            });


            await Task.FromResult(0);
        }

        public async Task TakeBalanceSnapshot()
        {
            try
            {
                var pairs = dbService.GetArbitragePairs("", baseExchange.Name, counterExchange.Name).Select(p => new ArbitragePair(p));

                var summaries = await baseExchange.MarketSummaries();
                var tickers = summaries.Where(s => s.BaseCurrency == "BTC" || s.QuoteCurrency == "BTC").ToDictionary(s => s.QuoteCurrency);

                var bs = await baseExchange.GetBalances();
                var cs = await counterExchange.GetBalances();

                var baseBalances = bs.ToDictionary(b => b.Currency);
                var counterBalances = cs.ToDictionary(c => c.Currency);

                //First take BTC snapshot
                if (tickers.ContainsKey("BTC"))
                {
                    InsertBalanceSnapshot(baseExchange, "USD", tickers["BTC"].Last, baseBalances, counterBalances);
                    InsertBalanceSnapshot(baseExchange, "BTC", 1, baseBalances, counterBalances);
                }
                else
                    logger.Error("No Ticker for BTC Balance Snapshot");

                foreach (var pair in pairs.Where(p => p.BaseCurrency == "BTC"))
                {
                    if (tickers.ContainsKey(pair.MarketCurrency))
                        InsertBalanceSnapshot(baseExchange, pair.MarketCurrency, tickers[pair.MarketCurrency].Last, baseBalances, counterBalances);
                    else
                        logger.Error("No Ticker for {0} Balance Snapshot", pair.MarketCurrency);
                }
            }
            catch (ApiException ae)
            {
                logger.Error(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(ae.Content), Formatting.Indented));
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private void InsertBalanceSnapshot(IExchange baseExchange, string currency, decimal price, Dictionary<string, ICurrencyBalance> baseBalances, Dictionary<string, ICurrencyBalance> counterBalances, int processId = 0)
        {
            decimal total = 0m;

            if (baseBalances.ContainsKey(currency))
            {
                total += baseBalances[currency].Available + baseBalances[currency].Held;
            }

            if (counterBalances.ContainsKey(currency))
            {
                total += counterBalances[currency].Available + counterBalances[currency].Held;
            }

            if (total < 0.01m) total = 0m;
            dbService.InsertBalanceSnapshot(currency, baseExchange.Name, counterExchange.Name, total, price, processId);
        }

        public void ProcessSnapshots(IEnumerable<ArbitragePair> pairs, ConcurrentDictionary<string, BalanceSnapshot> preSnapshot, ConcurrentDictionary<string, BalanceSnapshot> postSnapshot)
        {
            if (pairs.Any() && preSnapshot.Any() && postSnapshot.Any())
            {
                try
                {
                    decimal commission = 0m;

                    Dictionary<string, object> changes = new Dictionary<string, object>();

                    var summaries = baseExchange.MarketSummaries().Result.ToDictionary(s => s.MarketName);

                    var btcTicker = summaries["BTCUSD"];
                    var btcPre = preSnapshot["BTC"];
                    var btcPost = postSnapshot["BTC"];


                    var ethTicker = summaries["ETHUSD"];
                    var ethPre = preSnapshot["ETH"];
                    var ethPost = postSnapshot["ETH"];

                    foreach (var pair in pairs)
                    {
                        var ticker = summaries[pair.Symbol];

                        var marketPre = preSnapshot[pair.MarketCurrency];
                        var marketPost = postSnapshot[pair.MarketCurrency];

                        if (marketPre.Total != marketPost.Total)
                        {
                            decimal bPrice = (pair.BaseCurrency == "BTC") ? btcTicker.Last : ethTicker.Last;
                            decimal diff = (marketPost.Total - marketPre.Total) * ticker.Last * bPrice;
                            commission += diff;
                            changes.Add(pair.Symbol, new { Pre = marketPre.Total, Post = marketPost.Total, RawDiff = diff, Price = ticker.Last, Commission = diff });
                        }
                    }

                    if (btcPre.Total != btcPost.Total)
                    {
                        decimal diff = (btcPost.Total - btcPre.Total) * btcTicker.Last;
                        commission += diff;
                        changes.Add("BTC", new { Pre = btcPre.Total, Post = btcPost.Total, RawDiff = diff, Price = btcTicker.Last, Commission = diff });
                    }

                    if (ethPre.Total != ethPost.Total)
                    {
                        decimal diff = (ethPost.Total - ethPre.Total) * ethTicker.Last;
                        commission += diff;
                        changes.Add("ETH", new { Pre = ethPre.Total, Post = ethPost.Total, RawDiff = diff, Price = ethTicker.Last, Commission = diff });
                    }

                    if (changes.Any())
                    {
                        changes.Add("TOTAL", new { Pre = ethPre.Total, Post = ethPost.Total, RawDiff = 0, Price = 0, Commission = commission });

                        logger.Trace("!!!SNAPSHOT!!!");
                        logger.Trace(JsonConvert.SerializeObject(changes, Formatting.Indented));
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
        }

        #region Task Logic
        public async Task ProcessOrderBook(IBaseSocketExchange baseExchange, ArbitragePair pair, CancellationToken cancellationToken)
        {
            var counterSocket = counterExchange.GetSocket();

            counterSockets[pair.Symbol] = counterSocket;

            counterSocket.OnBook += async (book) =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        decimal counterBuy = Helper.RoundPrice(pair, EngineHelper.GetPriceAtVolumeThreshold(pair.BidMultiplier * pair.TradeThreshold, book.asks));
                        decimal counterSell = Helper.RoundPrice(pair, EngineHelper.GetPriceAtVolumeThreshold(pair.AskMultiplier * pair.TradeThreshold, book.bids));

                        //Calculate the BID based off what we can sell for on the counter exchange
                        decimal buy = Helper.RoundPrice(pair, counterSell - (counterSell * pair.BidSpread));
                        decimal sell = Helper.RoundPrice(pair, counterBuy + (counterBuy * pair.AskSpread));

                        if (buy >= counterSell)
                        {
                            buy -= pair.TickSize;
                        }

                        if (sell <= counterBuy)
                        {
                            sell += pair.TickSize;
                        }

                        var tasks = new List<Task>
                        {
                            PlaceBuyIOC(baseExchange, pair, buy),
                            PlaceSellIOC(baseExchange, pair, sell)
                        };

                        await Task.WhenAll(tasks);
                    }
                    catch (AggregateException ae)
                    {
                        ae.Handle((x) =>
                        {
                            if (x is ApiException) // This we know how to handle.
                            {
                                logger.Error(((ApiException)x).Content);
                            }
                            return false; // Let anything else stop the application.
                        });
                    }
                }
            };

            counterSocket.SubscribeOrderbook(pair.CounterSymbol);
            await Task.FromResult(true);
        }

        public async Task ProcessTrade(IBaseSocketExchange baseExchange, ArbitragePair pair, IMatch trade)
        {
            if (trade.Symbol == pair.Symbol && trade.QuantityFilled > 0)
            {
                logger.Trace(JsonConvert.SerializeObject(trade, Formatting.Indented));

                await PlaceCounterOrder(pair, trade);
            }
        }

        private async Task PlaceBuyIOC(IBaseSocketExchange baseExchange, ArbitragePair pair, decimal price)
        {
            decimal buyQuantity = 0;

            try
            {
             //   decimal raw = pair.BidMultiplier * pair.TradeThreshold / price;
              //  buyQuantity = Helper.RoundQuantity(pair, pair.BidMultiplier * pair.TradeThreshold / price);

                buyQuantity = pair.Increment;

                if (buyQuantity > 0)
                {
                    var result = await baseExchange.ImmediateOrCancel("buy", pair.BaseSymbol, buyQuantity, price);
                    Console.WriteLine(string.Format("IOK  BUY {0} {1} {2:P2}", pair.Symbol, price, pair.BidSpread));
                }
                else
                {
                    logger.Error("No Quantity for {0}", pair.Symbol);
                }
            }
            catch (ApiException ae)
            {
                if (ae.Content.ToLowerInvariant().Contains("insufficient"))
                {
                    pair.Status = "error";
                    dbService.SaveArbitragePair(pair);
                }
                logger.Error(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(ae.Content), Formatting.Indented));
            }
            catch (Exception e)
            {
                logger.Error(e);
                Thread.Sleep(1000 * 10);
            }

        }

        private async Task PlaceSellIOC(IBaseSocketExchange baseExchange, ArbitragePair pair, decimal price)
        {
            decimal sellQuantity = 0;
            try
            {
                //  decimal raw = pair.AskMultiplier * pair.TradeThreshold / price;
                //  sellQuantity = Helper.RoundQuantity(pair, pair.AskMultiplier * pair.TradeThreshold / price);
                sellQuantity = pair.Increment;

                if (sellQuantity > 0)
                {
                    var result = await baseExchange.ImmediateOrCancel("sell", pair.BaseSymbol, sellQuantity, price);
                    Console.WriteLine(string.Format("IOK SELL {0} {1} {2:P2}", pair.Symbol, price, pair.AskSpread));
                }
                else
                {
                    logger.Error("No Quantity for {0}", pair.Symbol);
                }
            }
            catch (ApiException ae)
            {
                if (ae.Content.ToLowerInvariant().Contains("insufficient"))
                {
                    pair.Status = "error";
                    dbService.SaveArbitragePair(pair);
                }
                logger.Error(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(ae.Content), Formatting.Indented));
            }
            catch (Exception e)
            {
                logger.Error(e);
                Thread.Sleep(1000 * 10);
            }
        }

        private async Task PlaceCounterOrder(ArbitragePair pair, IMatch trade)
        {
            try
            {
                decimal quantity = Helper.RoundQuantity(pair, trade.QuantityFilled);

                if (trade.Side == OrderSide.Sell)
                {
                    var result = await counterExchange.MarketBuy(trade.Symbol, quantity);
                    logger.Trace(string.Format("COUNTER BUY {0} {1} {2} {3}", pair.CounterSymbol, quantity, trade.Uuid, result.Uuid));
                    dbService.InsertMakerOrder(pair.Id, "basesell", trade.Uuid, result.Uuid);
                }
                else if (trade.Side == OrderSide.Buy)
                {
                    var result = await counterExchange.MarketSell(trade.Symbol, quantity);
                    logger.Trace(string.Format("COUNTER SELL {0} {1} {2} {3}", pair.CounterSymbol, quantity, trade.Uuid, result.Uuid));
                    dbService.InsertMakerOrder(pair.Id, "basebuy", trade.Uuid, result.Uuid);
                }
            }
            catch (ApiException ae)
            {
                if (ae.Content.ToLowerInvariant().Contains("insufficient"))
                {
                    pair.Status = "error";
                    dbService.SaveArbitragePair(pair);
                }
                logger.Error(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(ae.Content), Formatting.Indented));
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
        #endregion 

        #region TPL Helpers

        private void Reset(IBaseSocketExchange baseExchange)
        {
            
            foreach(var socket in counterSockets.Values)
            {
                socket.Reset();
            }

            counterSockets.Clear();

            baseExchange.Reset();
        }


        ITargetBlock<IBaseSocketExchange> CreateNeverEndingTask(
     Func<IBaseSocketExchange, CancellationToken, Task> action,
     CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (action == null) throw new ArgumentNullException("action");

            // Declare the block variable, it needs to be captured.
            ActionBlock<IBaseSocketExchange> block = null;

            // Create the block, it will call itself, so
            // you need to separate the declaration and
            // the assignment.
            // Async so you can wait easily when the
            // delay comes.
            block = new ActionBlock<IBaseSocketExchange>(async exchange =>
            {
                // Perform the action.  Wait on the result.
                await action(exchange, cancellationToken).
                    // Doing this here because synchronization context more than
                    // likely *doesn't* need to be captured for the continuation
                    // here.  As a matter of fact, that would be downright
                    // dangerous.
                    ConfigureAwait(false);

                // Wait.
                await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken).
                // Same as above.
                 ConfigureAwait(false);

                Reset(exchange);

                //TakeBalanceSnapshot();

                await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken).
                // Same as above.
                 ConfigureAwait(false);

                // Post the action back to the block.
                block.Post(exchange);
                //block.Complete();
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            // Return the block.
            return block;
        }

        //TODO: Create Factory for DbTransaction processer
        #endregion

    }
}