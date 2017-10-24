using System;
using System.Collections.Generic;
using BMCore.Config;

namespace BMCore.DbService
{

    public interface IDbService
    {
        void LogTrade(string baseX, string arbX, string symbol, string runType, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold, int processId);

        IEnumerable<DbTradeLog> GetTrades(int limit = 100);

        void LogError(string baseX, string arbX, string symbol, string method, Exception ex, int processId = -1);


        IEnumerable<DbOrder> GetOrders(long id = -1, string uuid = null, string status = "");
        IEnumerable<DbWithdrawOrder> GetOrdersToWithdraw(string fromExchange, string toExchange, string side, decimal threshold);
        void CloseOrders(IEnumerable<long> ids);
        long InsertOrder(string exchange, string symbol, string baseCurrency, string marketCurrency, string side, int processId, decimal price);
        void UpdateOrderUuid(long id, string uuid);
        void FillOrder(long id, decimal quantity, decimal price, decimal rate, decimal fee = 0m);
        long InsertWithdrawal(string uuid, long orderId, string currency, string fromExchange, decimal amount, int processId);
        void CloseWithdrawal(long id, decimal actualAmount, string txId);

        IEnumerable<DbWithdrawal> GetWithdrawals(long id = -1, string uuid = null, string status = "");

        void SaveOrderPair(long orderId1, string exchange1, long orderId2, string exchange2);
        void SaveWithdrawalPair(long withdrawalId1, long withdrawalId2);

        void UpdateOrderStatus(long id, string status, Exception e = null);
        void UpdateWithdrawalStatus(long id, string status, Exception e = null);

        int StartEngineProcess(string baseExchange, string arbExchange, string runType, CurrencyConfig baseCurrency);
        void EndEngineProcess(int id, string resultStatus, object payload = null);

        int GetInvalidOrderCount();

        void InsertArbitragePair(string baseExchange, string arbExchange, string symbol, string baseSymbol, string counterSymbol, string baseCurrency, string marketCurrency);

        IEnumerable<DbArbitragePair> GetArbitragePairs(string status, string baseExchange = "", string arbExchange = "");

        void UpdateArbitragePair(string baseExchange, string arbExchange, string symbol, bool isTrade = false, bool isError = false, bool isOpportunity = false, object payload = null);
        void UpdateArbitragePairById(int id, bool isTrade = false, bool isError = false, bool isOpportunity = false, bool isFunded = false, object payload = null);
        void InsertArbitrageOpportunity(int pairId, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold);

    }
}