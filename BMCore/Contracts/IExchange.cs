using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCore.Models;

namespace BMCore.Contracts
{
    public interface IExchange
    {
        decimal Fee
        {
            get;
        }

        string Name
        {
            get;
        }

        //Public Market API Endpoints 
        Task<IEnumerable<IMarket>> MarketSummaries();

        Task<OrderBook> OrderBook(string symbol);

        Task<IMarket> MarketSummary(string symbol);



        //Authenticated API Endpoints
        Task<IAcceptedAction> Buy(string symbol, decimal quantity, decimal rate);

        Task CancelOrder(string orderId);

        Task<IAcceptedAction> Sell(string symbol, decimal quantity, decimal rate);

        Task<IOrder> CheckOrder(string uuid);

        Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null);

        Task<IDepositAddress> GetDepositAddress(string currency);

        Task<ICurrencyBalance> GetBalance(string currency);

    }

}