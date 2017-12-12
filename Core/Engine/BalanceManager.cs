using Core.Contracts;
using Core.DbService;
using Core.Domain;
using Core.Models;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Core.Engine
{
    public class BalanceManager
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        IDbService dbService;
        IExchange baseExchange;
        IExchange counterExchange;

        private ConcurrentDictionary<string, ICurrencyBalance> baseBalances;
        private ConcurrentDictionary<string, ICurrencyBalance> counterBalances;

        private ConcurrentDictionary<string, string> activeWithdrawals;
        private ConcurrentDictionary<string, bool> completedPairs;

        public BalanceManager(IDbService dbService, IExchange baseExchange, IExchange counterExchange)
        {
            this.dbService = dbService;
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;

            baseBalances = new ConcurrentDictionary<string, ICurrencyBalance>();
            counterBalances = new ConcurrentDictionary<string, ICurrencyBalance>();
            activeWithdrawals = new ConcurrentDictionary<string, string>();
            completedPairs = new ConcurrentDictionary<string, bool>();
        }

        public void StartManager()
        {
            Console.WriteLine("Starting Balance Manager");

            var wtoken = new CancellationTokenSource();

            // Set the tasks.
            var task = (ActionBlock<IEnumerable<ArbitragePair>>)CreateNeverEndingTask((e, ct) => DoWork(e, ct), wtoken.Token);

            var pairs = dbService.GetArbitragePairs("", baseExchange.Name, counterExchange.Name).Select(p => new ArbitragePair(p));

            task.Post(pairs);
        }

        public async Task DoWork(IEnumerable<ArbitragePair> pairs, CancellationToken token)
        {
            try
            {
                Console.WriteLine("Balancing Pairs");

                baseBalances.Clear();
                counterBalances.Clear();
                completedPairs.Clear();

                var bBalances = await baseExchange.GetBalances();
                var cBalances = await counterExchange.GetBalances();

                foreach (var b in bBalances)
                {
                    //Prevent balancing crumbs
                    if(b.Available > 0.01m)
                        baseBalances.TryAdd(b.Currency, b);
                }

                foreach (var b in cBalances)
                {
                    //Prevent balancing crumbs
                    if (b.Available > 0.01m)
                        counterBalances.TryAdd(b.Currency, b);
                }


          //      BalanceCurrency("BTC");
         //       BalanceCurrency("ETH");

                foreach (var pair in pairs)
                {
                    try
                    {
                        if(baseBalances.ContainsKey(pair.MarketCurrency) && counterBalances.ContainsKey(pair.MarketCurrency) && !completedPairs.ContainsKey(pair.MarketCurrency))
                        {
                          //  BalanceCurrency(pair.MarketCurrency);
                        }
                        AuditCompletedOrdersForPair(pair, token);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(new { Exception = ex.Message, pair = pair.Symbol});
                    }

                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }

        }

        public void BalanceCurrency(string currency)
        {
            var baseBalance = baseBalances[currency];
            var counterBalance = counterBalances[currency];
            decimal targetBalance = baseBalance.Available + counterBalance.Available / 2;

            decimal diff = 2 * (baseBalance.Available - counterBalance.Available) / (baseBalance.Available + counterBalance.Available);

            Console.WriteLine("{0}: {1} Balance {2}, {3} Balance {4}, Diff {5}", currency, baseExchange.Name, baseBalance.Available, counterExchange.Name, counterBalance.Available, diff);

            if(activeWithdrawals.ContainsKey(currency) && Math.Abs(diff) < 0.5m)
            {
                string wId;
                activeWithdrawals.Remove(currency, out wId);
                logger.Trace("Completed Withdrawal {0}", currency);
            }

            //If diff < 0 Base needs more funds, depost into base
            if(!activeWithdrawals.ContainsKey(currency) && Math.Abs(diff) > 1.5m && diff < 0)
            {
                var address = baseExchange.GetDepositAddress(currency).Result;
                decimal amount = targetBalance - baseBalance.Available;
                var result = counterExchange.Withdraw(address.Currency, amount, address.Address, address.Tag).Result;
                logger.Trace("Withdrawal {0}", currency);
                activeWithdrawals.TryAdd(currency, result.Uuid);
            }
            if (!activeWithdrawals.ContainsKey(currency) && Math.Abs(diff) > 1.5m && diff > 0)
            {
                var address = counterExchange.GetDepositAddress(currency).Result;
                decimal amount = targetBalance - baseBalance.Available;
                var result = baseExchange.Withdraw(address.Currency, amount, address.Address, address.Tag).Result;
                logger.Trace("Withdrawal {0}", currency);
                activeWithdrawals.TryAdd(currency, result.Uuid);
            }

            completedPairs.TryAdd(currency, true);
        }

        public void AuditCompletedOrdersForPair(ArbitragePair pair, CancellationToken cancellationToken)
        {
            var orders = dbService.GetMakerOrdersByStatus("filled", pair.Id).Where(o => o.BaseExchange == baseExchange.Name && o.CounterExchange == counterExchange.Name);

            if(orders.Any())
                logger.Trace("Audit {0} orders for {1}", orders.Count(), pair.Symbol);

            foreach (var order in orders)
            {
                try
                {
                    var baseOrder = baseExchange.CheckOrder(order.BaseOrderUuid, pair.Symbol).Result;
                    var counterOrder = counterExchange.CheckOrder(order.CounterOrderUuid, pair.Symbol).Result;

                    if (baseOrder != null && counterOrder != null)
                    {
                        decimal commission = 0m;
                        if (baseOrder.Side == OrderSide.Buy)
                        {
                            commission = counterOrder.CostProceeds - baseOrder.CostProceeds;
                        }
                        else
                        {
                            commission = baseOrder.CostProceeds - counterOrder.CostProceeds;
                        }

                        dbService.UpdateOrder(order.Id, baseRate: baseOrder.AvgRate, counterRate: counterOrder.AvgRate, baseCost: baseOrder.CostProceeds, counterCost: counterOrder.CostProceeds, commission: commission, status: "complete", baseQuantityFilled: baseOrder.QuantityFilled, counterQuantityFilled: counterOrder.QuantityFilled);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
        }

        ITargetBlock<IEnumerable<ArbitragePair>> CreateNeverEndingTask(
    Func<IEnumerable<ArbitragePair>, CancellationToken, Task> action,
    CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (action == null) throw new ArgumentNullException("action");

            // Declare the block variable, it needs to be captured.
            ActionBlock<IEnumerable<ArbitragePair>> block = null;

            // Create the block, it will call itself, so
            // you need to separate the declaration and
            // the assignment.
            // Async so you can wait easily when the
            // delay comes.
            block = new ActionBlock<IEnumerable<ArbitragePair>>(async pairs =>
            {
                // Perform the action.  Wait on the result.
                await action(pairs, cancellationToken).
                    // Doing this here because synchronization context more than
                    // likely *doesn't* need to be captured for the continuation
                    // here.  As a matter of fact, that would be downright
                    // dangerous.
                    ConfigureAwait(false);

                // Wait.
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken).
                // Same as above.
                 ConfigureAwait(false);

                pairs = dbService.GetArbitragePairs("", baseExchange.Name, counterExchange.Name).Select(p => new ArbitragePair(p));
                // Post the action back to the block.
                block.Post(pairs);
                //block.Complete();
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            // Return the block.
            return block;
        }
    }
}
