using System;
using System.Collections.Generic;

namespace BMCore.DbService
{

    public interface IDbService
    {
        void LogTrade(string baseX, string arbX, string symbol, string runType, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold);

        IEnumerable<DbTradeLog> GetTrades(int limit = 100);

        void LogError(string baseX, string arbX, string symbol, string method, string message, string stackTrace);


        IEnumerable<DbOrder> GetOrders(long id = -1, string uuid = null, string status = "");
        long InsertOrder(string exchange, string symbol, string baseCurrency, string marketCurrency, string side);
        void UpdateOrderUuid(long id, string uuid, string status = "", long counterId = 0, decimal quantity = 0m, decimal price = 0m, decimal commission = 0m);
        void FillOrder(long id, decimal quantity, decimal price, decimal fee = 0m);
        long InsertWithdrawal(string uuid, long orderId, string currency, string fromExchange, decimal amount);
        void CloseWithdrawal(long id, decimal actualAmount, string txId);

        IEnumerable<DbWithdrawal> GetWithdrawals(long id = -1, string uuid = null, string status = "");

        void SaveOrderPair(long orderId1, long orderId2);
        void SaveWithdrawalPair(long withdrawalId1, long withdrawalId2);

        void UpdateOrderStatus(long id, string status, Exception e = null);
        void UpdateWithdrawalStatus(long id, string status, Exception e = null);

        int StartEngineProcess(string baseExchange, string arbExchange, string runType);
        void EndEngineProcess(int id, string resultStatus, object payload = null);
    }
}