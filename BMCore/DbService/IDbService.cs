using System;
using System.Collections.Generic;
using BMCore.Config;

namespace BMCore.DbService
{

    public interface IDbService
    {
        void LogError(string baseX, string arbX, string symbol, string method, Exception ex, int processId = -1);
        int StartEngineProcess(string baseExchange, string arbExchange, string runType, CurrencyConfig baseCurrency);
        void EndEngineProcess(int id, string resultStatus, object payload = null);

        void InsertArbitragePair(string baseExchange, string arbExchange, string symbol, string baseSymbol, string counterSymbol, string baseCurrency, string marketCurrency);
        IEnumerable<DbArbitragePair> GetArbitragePairs(string type, string status = "", string baseExchange = "", string arbExchange = "");
        void UpdateArbitragePair(string baseExchange, string arbExchange, string symbol, bool isTrade = false, bool isError = false, bool isOpportunity = false, object payload = null);
        void UpdateArbitragePairById(int id, bool isTrade = false, bool isError = false, bool isOpportunity = false, bool isFunded = false, object payload = null);
        void InsertArbitrageOpportunity(int pairId, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold);

        int InsertTransaction(int pairId, string type);
        void UpdateTransactionStatus(int id, string status, object payload = null);
        void UpdateTransactionOrderUuid(int id, string baseUuid, string counterUuid, object payload = null);
        void UpdateTransactionWithdrawalUuid(int id, string baseUuid, string counterUuid, decimal commission);
        void CloseTransaction(int id, string baseTxId, string counterTxId);
        IEnumerable<DbTransaction> GetTransactions(string status, int pairId = 0);

        int InsertMakerOrder(int pairId, string type, decimal rate, int pId);
        void UpdateOrderUuid(int id, string baseUuid, string counterUuid = "", decimal counterRate = 0m, string status = "");
        void UpdateOrderStatus(int id, string status, decimal baseQuantityFilled = 0m, decimal counterQuantityFilled = 0m);
        void UpdateOrder(int id, string baseUuid = "", string counterUuid = "", decimal baseRate = 0m, decimal counterRate = 0m, string status = "", decimal baseQuantityFilled = 0m, decimal counterQuantityFilled = 0m, decimal baseCost = 0m, decimal counterCost = 0m, decimal commission = 0m, object payload = null);
        IEnumerable<DbMakerOrder> GetMakerOrdersByStatus(string status, int pairId = 0);

        void InsertBalanceSnapshot(string currency, string exchange, decimal available, decimal held, decimal price, int processId);
        IEnumerable<DbBalance> GetBalanceSnapshots(string exchange = "", string currency = "", DateTime? fromDate = null, int processId = 0);

    }
}