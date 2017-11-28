using Core.Config;
using Core.Contracts;
using Core.DbService;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Core.Engine
{
    public class SocketEngine
    {

        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        IExchange baseExchange;
        IExchange counterExchange;
        IDbService dbService;

        GmailConfig gmail;

        CancellationTokenSource wtoken;
        ActionBlock<DateTimeOffset> task;

        public SocketEngine(IExchange baseExchange, IExchange counterExchange, IDbService dbService, GmailConfig gmail)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
            this.gmail = gmail;
        }

        void StartWork()
        {
            // Create the token source.
            wtoken = new CancellationTokenSource();

            // Set the task.
            task = (ActionBlock<DateTimeOffset>)CreateNeverEndingTask((now, ct) => DoWorkAsync(ct), wtoken.Token);

            // Start the task.  Post the time.
            task.Post(DateTimeOffset.Now);
        }

        void StopWork()
        {
            // CancellationTokenSource implements IDisposable.
            using (wtoken)
            {
                // Cancel.  This will cancel the task.
                wtoken.Cancel();
            }

            // Set everything to null, since the references
            // are on the class level and keeping them around
            // is holding onto invalid state.
            wtoken = null;
            task = null;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
           await Task.FromResult(0);
        }

        ITargetBlock<DateTimeOffset> CreateNeverEndingTask(
     Func<DateTimeOffset, CancellationToken, Task> action,
     CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (action == null) throw new ArgumentNullException("action");

            // Declare the block variable, it needs to be captured.
            ActionBlock<DateTimeOffset> block = null;

            // Create the block, it will call itself, so
            // you need to separate the declaration and
            // the assignment.
            // Async so you can wait easily when the
            // delay comes.
            block = new ActionBlock<DateTimeOffset>(async now => {
                // Perform the action.  Wait on the result.
                await action(now, cancellationToken).
                    // Doing this here because synchronization context more than
                    // likely *doesn't* need to be captured for the continuation
                    // here.  As a matter of fact, that would be downright
                    // dangerous.
                    ConfigureAwait(false);

                // Wait.
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken).
                    // Same as above.
                    ConfigureAwait(false);

                // Post the action back to the block.
                block.Post(DateTimeOffset.Now);
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            // Return the block.
            return block;
        }
    }
}
