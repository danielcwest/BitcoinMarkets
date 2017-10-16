using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BMCore.DbService
{
    public class BMDbService : IDbService
    {
        private string sqlConnectionString = string.Empty;

        // constructor
        public BMDbService(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public void LogTrade(string baseX, string arbX, string symbol, decimal basePrice, decimal arbPrice, decimal spread)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertTrade", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseX },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbX },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@basePrice", Value = basePrice },
                    new SqlParameter { ParameterName = "@arbPrice", Value = arbPrice},
                    new SqlParameter { ParameterName = "@spread", Value = spread}
                });
        }

        public IEnumerable<DbTradeLog> GetTrades(int limit = 100)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetTrades", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@count", Value = limit }
                }))
            {
                return reader.ToList<DbTradeLog>();
            }
        }

        public void LogError(string baseX, string arbX, string symbol, string method, string message, string stackTrace)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertError", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseX },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbX },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@method", Value = method },
                    new SqlParameter { ParameterName = "@message", Value = message},
                    new SqlParameter { ParameterName = "@exception", Value = stackTrace}
                });
        }

        public long InsertOrder(string exchange, string symbol, string baseCurrency, string marketCurrency, string side)
        {
            return (long)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.InsertOrder", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@exchange", Value = exchange },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@side", Value = side },
                    new SqlParameter { ParameterName = "@baseCurrency", Value = baseCurrency },
                    new SqlParameter { ParameterName = "@marketCurrency", Value = marketCurrency }
                });
        }

        public long InsertWithdrawal(string uuid, long orderId, string currency, string fromExchange, decimal amount)
        {
            return (long)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.InsertWithdrawal", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@uuid", Value = uuid },
                    new SqlParameter { ParameterName = "@orderId", Value = orderId },
                    new SqlParameter { ParameterName = "@currency", Value = currency },
                    new SqlParameter { ParameterName = "@fromExchange", Value = fromExchange },
                    new SqlParameter { ParameterName = "@amount", Value = amount },
                });
        }

        public void UpdateOrderUuid(long id, string uuid, string status = "", long counterId = 0, decimal quantity = 0m, decimal price = 0m, decimal commission = 0m)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateOrderUuid", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@counterId", Value = counterId },
                    new SqlParameter { ParameterName = "@uuid", Value = uuid },
                    new SqlParameter { ParameterName = "@status", Value = status },
                    new SqlParameter { ParameterName = "@quantity", Value = quantity},
                    new SqlParameter { ParameterName = "@price", Value = price},
                    new SqlParameter { ParameterName = "@commission", Value = commission}
                });
        }

        public void FillOrder(long id, decimal quantity, decimal price, decimal fee = 0)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.CloseOrder", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@quantity", Value = quantity },
                    new SqlParameter { ParameterName = "@price", Value = price },
                    new SqlParameter { ParameterName = "@commission", Value = fee }
                });
        }

        public void CloseWithdrawal(long id, decimal actualAmount, string txId)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.CloseWithdrawal", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@amount", Value = actualAmount },
                    new SqlParameter { ParameterName = "@txid", Value = txId }
                });
        }

        public IEnumerable<DbOrder> GetOrders(long id = -1, string uuid = null, string status = "")
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetOrders", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@id", Value = id },
                        new SqlParameter { ParameterName = "@uuid", Value = uuid },
                        new SqlParameter { ParameterName = "@status", Value = status }
                }))
            {
                return reader.ToList<DbOrder>();
            }
        }

        public IEnumerable<DbWithdrawal> GetWithdrawals(long id = -1, string uuid = null, string status = "")
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetWithdrawals", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@id", Value = id },
                        new SqlParameter { ParameterName = "@uuid", Value = uuid },
                        new SqlParameter { ParameterName = "@status", Value = status }
                }))
            {
                return reader.ToList<DbWithdrawal>();
            }
        }

        public void SaveOrderPair(long orderId1, long orderId2)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.SaveOrderPair", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@order1Id", Value = orderId1 },
                    new SqlParameter { ParameterName = "@order2Id", Value = orderId2 }
                });
        }

        public void SaveWithdrawalPair(long withdrawalId1, long withdrawalId2)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.SaveWithdrawalPair", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@order1Id", Value = withdrawalId1 },
                    new SqlParameter { ParameterName = "@order2Id", Value = withdrawalId2 }
                });
        }

        public void UpdateOrderStatus(long id, string status, Exception e = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateOrderStatus", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@status", Value = status },
                    new SqlParameter { ParameterName = "@meta", Value = (e == null) ? "" : e.ToString()}
                });
        }

        public void UpdateWithdrawalStatus(long id, string status, Exception e = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateWithdrawalStatus", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@status", Value = status },
                    new SqlParameter { ParameterName = "@meta", Value = (e == null) ? "" : e.ToString()}
                });
        }
    }

}