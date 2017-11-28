using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface ISocketExchange : IDisposable //TODO: Make IDisposable
    {
        string Name
        {
            get;
        }

        IExchange Rest
        {
            get;
        }

        event Action<OrderBook> OnBook;

        event Action<IMatch> OnMatch;

        void SubscribeOrderbook(string symbol);

        void SubscribeTrades(string symbol);

        Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price);

    }
}
