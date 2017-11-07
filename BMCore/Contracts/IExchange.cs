using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMCore.Contracts
{
    public interface IExchange
    {
        string Name
        {
            get;
        }

        //Public Market API Endpoints 
        Task<IEnumerable<ISymbol>> Symbols();

        Task<Models.OrderBook> OrderBook(string symbol);

        Task<ITicker> Ticker(string symbol);

        //Authenticated API Endpoints
        Task<IAcceptedAction> LimitBuy(string generatedId, string symbol, decimal quantity, decimal price);
        Task<IAcceptedAction> LimitSell(string generatedId, string symbol, decimal quantity, decimal price);
        Task<IAcceptedAction> MarketBuy(string generatedId, string symbol, decimal quantity);
        Task<IAcceptedAction> MarketSell(string generatedId, string symbol, decimal quantity);

        Task CancelOrder(string orderId);
        Task<IEnumerable<string>> CancelOrders(string symbol);
        Task<IOrder> CheckOrder(string uuid);

        Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null);
        Task<IWithdrawal> GetWithdrawal(string uuid);
        Task<IDepositAddress> GetDepositAddress(string currency);

        Task<IEnumerable<ICurrencyBalance>> GetBalances();
    }

}