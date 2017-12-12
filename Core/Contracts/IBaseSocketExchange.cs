using Core.Handlers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IBaseSocketExchange : IDisposable
    {
        string Name
        {
            get;
        }

        IExchange Rest
        {
            get;
        }

      //  void SubscribeOrderbook(string symbol);

       void RegisterPair(ArbitragePair pair, SocketTradeHandler handler);

        void SubscribeTrades(CancellationToken token);

        void Reset();

        Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price);

    }
}
