using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IExchange
    {
        string Name
        {
            get;
        }

        //Public Market API Endpoints 
        Task<IEnumerable<ISymbol>> Symbols();
        Task<IEnumerable<ITicker>> MarketSummaries();

        Task<Models.OrderBook> OrderBook(string symbol);

        Task<ITicker> Ticker(string symbol);

        //Authenticated API Endpoints
        Task<IAcceptedAction> LimitBuy(string symbol, decimal quantity, decimal price);
        Task<IAcceptedAction> LimitSell(string symbol, decimal quantity, decimal price);
        Task<IAcceptedAction> MarketBuy(string symbol, decimal quantity);
        Task<IAcceptedAction> MarketSell(string symbol, decimal quantity);
        Task<IAcceptedAction> FillOrKill(string side, string symbol, decimal quantity, decimal price);
        Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price);

        Task CancelOrder(string orderId);
        Task<IEnumerable<string>> CancelOrders(string symbol);
        Task<IOrder> CheckOrder(string uuid, string symbol = "");

        Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null);
        Task<IWithdrawal> GetWithdrawal(string uuid);
        Task<IDepositAddress> GetDepositAddress(string currency);

        Task<IEnumerable<ICurrencyBalance>> GetBalances();

        ISocketExchange GetSocket();
    }

}