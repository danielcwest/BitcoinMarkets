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
    public class HitbtcBinanceEngine
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        IExchange hitbtc;
        IExchange binance;

        ConcurrentDictionary<string, ISocketExchange> counterSockets;

        IDbService dbService;

        ConcurrentDictionary<string, CancellationTokenSource> tokens;

        public HitbtcBinanceEngine(IExchange hitbtc, IExchange binance, IDbService dbService)
        {
            this.hitbtc = hitbtc;
            this.binance = binance;
            this.dbService = dbService;

            counterSockets = new ConcurrentDictionary<string, ISocketExchange>();
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

    }
}
