using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BMCore.DbService
{
    public class DbService
    {
        private string sqlConnectionString = string.Empty;

        // constructor
        public DbService(string sqlConnectionString)
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
    }

}