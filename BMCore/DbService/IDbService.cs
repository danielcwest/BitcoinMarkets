using System;
using System.Collections.Generic;

namespace BMCore.DbService
{

    public interface IDbService
    {
        void LogTrade(string baseX, string arbX, string symbol, string runType, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold);

        IEnumerable<DbTradeLog> GetTrades(int limit = 100);

        void LogError(string baseX, string arbX, string symbol, string method, Exception ex);


        IEnumerable<DbOrder> GetOrders(long id = -1, string uuid = null, string status = "");
        IEnumerable<DbWithdrawOrder> GetOrdersToWithdraw(string fromExchange, string toExchange, string side, decimal threshold);
        void CloseOrders(IEnumerable<long> ids);
        long InsertOrder(string exchange, string symbol, string baseCurrency, string marketCurrency, string side, int processId);
        void UpdateOrderUuid(long id, string uuid);
        void FillOrder(long id, decimal quantity, decimal price, decimal rate, decimal fee = 0m);
        long InsertWithdrawal(string uuid, long orderId, string currency, string fromExchange, decimal amount, int processId);
        void CloseWithdrawal(long id, decimal actualAmount, string txId);

        IEnumerable<DbWithdrawal> GetWithdrawals(long id = -1, string uuid = null, string status = "");

        void SaveOrderPair(long orderId1, string exchange1, long orderId2, string exchange2);
        void SaveWithdrawalPair(long withdrawalId1, long withdrawalId2);

        void UpdateOrderStatus(long id, string status, Exception e = null);
        void UpdateWithdrawalStatus(long id, string status, Exception e = null);

        int StartEngineProcess(string baseExchange, string arbExchange, string runType);
        void EndEngineProcess(int id, string resultStatus, object payload = null);

        int GetInvalidOrderCount();
    }
}