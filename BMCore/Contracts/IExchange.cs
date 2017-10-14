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
        Task<IEnumerable<ISymbol>> Symbols();

        Task<IEnumerable<IMarket>> MarketSummaries();

        Task<OrderBook> OrderBook(string symbol);

        Task<IMarket> MarketSummary(string symbol);



        //Authenticated API Endpoints
        Task<IAcceptedAction> Buy(string generatedId, string symbol, decimal quantity, decimal rate, decimal lot = 1.0m);
        Task<IAcceptedAction> Sell(string generatedId, string symbol, decimal quantity, decimal rate, decimal lot = 1.0m);

        Task CancelOrder(string orderId);
        Task<IOrder> CheckOrder(string uuid, decimal lot = 1.0m);

        Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null);
        Task<IWithdrawal> GetWithdrawal(string uuid);
        Task<IDepositAddress> GetDepositAddress(string currency);

        Task<ICurrencyBalance> GetBalance(string currency);

    }

}