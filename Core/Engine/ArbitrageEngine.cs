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

namespace Core.Engine
{
    public class ArbitrageEngine
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        IExchange baseExchange;
        IExchange counterExchange;
        ConcurrentDictionary<string, ISocketExchange> baseSockets;
        ConcurrentDictionary<string, ISocketExchange> counterSockets;

        IDbService dbService;

        ConcurrentDictionary<string, CancellationTokenSource> tokens;

        public ArbitrageEngine(IExchange baseExchange, IExchange counterExchange, IDbService dbService)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
            baseSockets = new ConcurrentDictionary<string, ISocketExchange>();
            counterSockets = new ConcurrentDictionary<string, ISocketExchange>();
        }

        public void StartEngine(CancellationToken token, int timeoutSeconds)
        {
            tokens = new ConcurrentDictionary<string, CancellationTokenSource>();

            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("ArbitrageEngine Starting ...");

                var pairs = dbService.GetArbitragePairs("market", baseExchange.Name, counterExchange.Name).Select(p => new ArbitragePair(p));

                foreach (var pair in pairs)
                {
                    StartWorkForPair(pair);
                }

                Thread.Sleep(TimeSpan.FromSeconds(timeoutSeconds));

                Console.WriteLine("ArbitrageEngine Shutting down ...");

                foreach (var pair in pairs)
                {
                    EndWorkForPair(pair);
                }

                TakeBalanceSnapshot().Wait();

                CloseConnections();
            }
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
                    InsertBalanceSnapshot("USD", tickers["BTC"].Last, baseBalances, counterBalances);
                    InsertBalanceSnapshot("BTC", 1, baseBalances, counterBalances);
                }
                else
                    logger.Error("No Ticker for BTC Balance Snapshot");

                foreach (var pair in pairs.Where(p => p.BaseCurrency == "BTC"))
                {
                    if (tickers.ContainsKey(pair.MarketCurrency))
                        InsertBalanceSnapshot(pair.MarketCurrency, tickers[pair.MarketCurrency].Last, baseBalances, counterBalances);
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

        private void InsertBalanceSnapshot(string currency, decimal price, Dictionary<string, ICurrencyBalance> baseBalances, Dictionary<string, ICurrencyBalance> counterBalances, int processId = 0)
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
        public async Task ProcessOrderBook(ArbitragePair pair, CancellationToken cancellationToken)
        {
            var counterSocket = counterSockets.ContainsKey(pair.Symbol) ? counterSockets[pair.Symbol] : counterExchange.GetSocket();

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
                            PlaceBuyIOC(pair, buy),
                            PlaceSellIOC(pair, sell)
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
                        EndWorkForPair(pair);
                    }
                }
            };

            counterSocket.SubscribeOrderbook(pair.Symbol);
            await Task.FromResult(true);
        }

        public async Task ProcessTrades(ArbitragePair pair, CancellationToken cancellationToken)
        {
            var baseSocket = baseSockets.ContainsKey(pair.Symbol) ? baseSockets[pair.Symbol] : baseExchange.GetSocket();

            baseSockets[pair.Symbol] = baseSocket;

            baseSocket.OnMatch += async (trade) =>
            {
                try
                {
                    if (trade.Symbol == pair.Symbol && trade.QuantityFilled > 0)
                    {
                        logger.Trace(JsonConvert.SerializeObject(trade, Formatting.Indented));

                        await PlaceCounterOrder(pair, trade);
                    }
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
                    EndWorkForPair(pair);
                }
            };

            baseSocket.SubscribeTrades(pair.Symbol);

            await Task.FromResult(true);
        }

        public async Task AuditCompletedOrdersForPair(ArbitragePair pair, CancellationToken cancellationToken)
        {
            foreach (var order in dbService.GetMakerOrdersByStatus("filled", pair.Id).Where(o => o.BaseExchange == baseExchange.Name && o.CounterExchange == counterExchange.Name))
            {
                try
                {
                    var baseOrder = await baseExchange.CheckOrder(order.BaseOrderUuid, pair.Symbol);
                    var counterOrder = await counterExchange.CheckOrder(order.CounterOrderUuid, pair.Symbol);

                    if (baseOrder != null && counterOrder != null)
                    {
                        decimal commission = 0m;
                        if (baseOrder.Side == OrderSide.buy)
                        {
                            commission = counterOrder.CostProceeds - baseOrder.CostProceeds;
                        }
                        else
                        {
                            commission = baseOrder.CostProceeds - counterOrder.CostProceeds;
                        }

                        dbService.UpdateOrder(order.Id, baseRate: baseOrder.AvgRate, counterRate: counterOrder.AvgRate, baseCost: baseOrder.CostProceeds, counterCost: counterOrder.CostProceeds, commission: commission, status: "complete", baseQuantityFilled: baseOrder.QuantityFilled, counterQuantityFilled: counterOrder.QuantityFilled);
                    }
                    // await EmailHelper.SendSimpleMailAsync(gmail, string.Format("Trade {0} - {1}", baseOrder.Symbol, commission), JsonConvert.SerializeObject(new { baseOrder = baseOrder, counterOrder = counterOrder }, Formatting.Indented));

                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
        }

        private async Task PlaceBuyIOC(ArbitragePair pair, decimal price)
        {
            decimal buyQuantity = 0;

            try
            {
                decimal raw = pair.BidMultiplier * pair.TradeThreshold / price;
                buyQuantity = Helper.RoundQuantity(pair, pair.BidMultiplier * pair.TradeThreshold / price);

                if (buyQuantity > 0)
                {
                    var result = await baseExchange.ImmediateOrCancel("buy", pair.BaseSymbol, buyQuantity, price);
                    Console.WriteLine(string.Format("LIMIT  BUY {0} {1} {2:P2}", pair.Symbol, price, pair.BidSpread));
                }
            }
            catch (ApiException ae)
            {
                EndWorkForPair(pair);

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

        private async Task PlaceSellIOC(ArbitragePair pair, decimal price)
        {
            decimal sellQuantity = 0;
            try
            {
                decimal raw = pair.AskMultiplier * pair.TradeThreshold / price;
                sellQuantity = Helper.RoundQuantity(pair, pair.AskMultiplier * pair.TradeThreshold / price);

                if (sellQuantity > 0)
                {
                    var result = await baseExchange.ImmediateOrCancel("sell", pair.BaseSymbol, sellQuantity, price);
                    Console.WriteLine(string.Format("LIMIT SELL {0} {1} {2:P2}", pair.Symbol, price, pair.AskSpread));
                }
            }
            catch (ApiException ae)
            {
                EndWorkForPair(pair);

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

        private async Task PlaceCounterOrder(ArbitragePair pair, IMatch trade)
        {
            try
            {
                decimal quantity = Helper.RoundQuantity(pair, trade.QuantityFilled);

                if (trade.Side == OrderSide.sell)
                {
                    var result = await counterExchange.MarketBuy(trade.Symbol, quantity);
                    logger.Trace(string.Format("COUNTER BUY {0} {1} {2} {3}", pair.Symbol, quantity, trade.Uuid, result.Uuid));
                    dbService.InsertMakerOrder(pair.Id, "basesell", trade.Uuid, result.Uuid);
                }
                else if (trade.Side == OrderSide.buy)
                {
                    var result = await counterExchange.MarketSell(trade.Symbol, quantity);
                    logger.Trace(string.Format("COUNTER SELL {0} {1} {2} {3}", pair.Symbol, quantity, trade.Uuid, result.Uuid));
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
        void StartWorkForPair(ArbitragePair pair)
        {
            // Create the token source.
            var wtoken = new CancellationTokenSource();

            // Set the tasks.
        //    var task1 = (ActionBlock<ArbitragePair>)CreateNeverEndingTask((p, ct) => ProcessOrderBook(p, ct), wtoken.Token);
           // var task2 = (ActionBlock<ArbitragePair>)CreateNeverEndingTask((p, ct) => ProcessTrades(p, ct), wtoken.Token);
            var task3 = (ActionBlock<ArbitragePair>)CreateNeverEndingTask((p, ct) => AuditCompletedOrdersForPair(p, ct), wtoken.Token);

            if (tokens.TryAdd(pair.Symbol, wtoken))
            {
                // Start the task.  Post the time.
            //    task1.Post(pair);
            //    task2.Post(pair);
                task3.Post(pair);
            }
        }

        void EndWorkForPair(ArbitragePair pair)
        {
            var wtoken = tokens[pair.Symbol];
            // CancellationTokenSource implements IDisposable.
            using (wtoken)
            {
                // Cancel.  This will cancel the task.
                wtoken.Cancel();
            }

            // Set everything to null, since the references
            // are on the class level and keeping them around
            // is holding onto invalid state.
            tokens.Remove(pair.Symbol, out wtoken);
            wtoken = null;
        }

        ITargetBlock<ArbitragePair> CreateNeverEndingTask(
     Func<ArbitragePair, CancellationToken, Task> action,
     CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (action == null) throw new ArgumentNullException("action");

            // Declare the block variable, it needs to be captured.
            ActionBlock<ArbitragePair> block = null;

            // Create the block, it will call itself, so
            // you need to separate the declaration and
            // the assignment.
            // Async so you can wait easily when the
            // delay comes.
            block = new ActionBlock<ArbitragePair>(async pair =>
            {
                // Perform the action.  Wait on the result.
                await action(pair, cancellationToken).
                    // Doing this here because synchronization context more than
                    // likely *doesn't* need to be captured for the continuation
                    // here.  As a matter of fact, that would be downright
                    // dangerous.
                    ConfigureAwait(false);

                // Wait.
                // await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken).
                // Same as above.
                // ConfigureAwait(false);

                // Post the action back to the block.
                //block.Post(pair);
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

        void CloseConnections()
        {
            foreach (var kvp in baseSockets)
            {
                kvp.Value.Dispose();
            }
            baseSockets.Clear();

            foreach (var kvp in counterSockets)
            {
                kvp.Value.Dispose();
            }
            counterSockets.Clear();
        }

    }
}