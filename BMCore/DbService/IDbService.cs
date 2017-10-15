using System;
using System.Collections.Generic;

namespace BMCore.DbService
{

    public interface IDbService
    {
        void LogTrade(string baseX, string arbX, string symbol, decimal basePrice, decimal arbPrice, decimal spread);

        IEnumerable<DbTradeLog> GetTrades(int limit = 100);

        void LogError(string baseX, string arbX, string symbol, string method, string message, string stackTrace);

        long InsertOrder(string exchange, string symbol, string baseCurrency, string marketCurrency, string side);

        IEnumerable<DbOrder> GetOrders(long id = -1, string uuid = null);

        void UpdateOrderUuid(long id, string uuid, string status, long counterId = 0, decimal quantity = 0m, decimal price = 0m, decimal commission = 0m);

        long InsertWithdrawal(string uuid, long orderId, string currency, string fromExchange, decimal amount);

        void UpdateWithdrawal(long id, long counterId, decimal actualAmount, string txId);

        IEnumerable<DbWithdrawal> GetWithdrawals(long id = -1, string uuid = null);
    }
}