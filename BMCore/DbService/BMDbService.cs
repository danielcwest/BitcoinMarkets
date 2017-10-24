using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using BMCore.Config;

namespace BMCore.DbService
{
    public class BMDbService : IDbService
    {
        private string sqlConnectionString = string.Empty;

        //TODO: Email stuff doesn't belong here, but its super nice for now
        GmailConfig gmail;
        // constructor
        public BMDbService(string sqlConnectionString, GmailConfig gmail = null)
        {
            this.gmail = gmail;
            this.sqlConnectionString = sqlConnectionString;
        }

        public void LogTrade(string baseX, string arbX, string symbol, string runType, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold, int processId)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertTrade", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseX },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbX },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@basePrice", Value = basePrice },
                    new SqlParameter { ParameterName = "@arbPrice", Value = arbPrice},
                    new SqlParameter { ParameterName = "@spread", Value = spread},
                    new SqlParameter { ParameterName = "@threshold", Value = threshold},
                    new SqlParameter { ParameterName = "@type", Value = runType},
                    new SqlParameter { ParameterName = "@processId", Value = processId}
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

        public void LogError(string baseX, string arbX, string symbol, string method, Exception ex, int processId = -1)
        {
            string message = ex.Message;
            string stackTrace = ex.StackTrace;
            if (ex.InnerException != null)
            {
                message = ex.InnerException.Message;
                message = ex.InnerException.StackTrace;
            }

            try
            {
                DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertError", 15,
                                new SqlParameter[]
                                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseX },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbX },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@method", Value = method },
                    new SqlParameter { ParameterName = "@message", Value = message},
                    new SqlParameter { ParameterName = "@exception", Value = stackTrace},
                    new SqlParameter { ParameterName = "@processId", Value = processId}
                                });

                if (gmail != null)
                    EmailHelper.SendSimpleMailAsync(gmail, string.Format("Error: {0}", ex.Message), string.Format("{0} - {1} {2} {3}", baseX, arbX, Environment.NewLine, stackTrace));
            }
            catch (Exception)
            {
                //fail silently
            }

        }

        public long InsertOrder(string exchange, string symbol, string baseCurrency, string marketCurrency, string side, int processId, decimal price)
        {
            if (gmail != null)
                EmailHelper.SendSimpleMailAsync(gmail, string.Format("New Order: {0} {1} {2}", exchange, side, symbol), string.Format("{0}", processId));

            return (long)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.InsertOrder", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@exchange", Value = exchange },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@side", Value = side },
                    new SqlParameter { ParameterName = "@baseCurrency", Value = baseCurrency },
                    new SqlParameter { ParameterName = "@marketCurrency", Value = marketCurrency },
                    new SqlParameter { ParameterName = "@pId", Value = processId },
                    new SqlParameter { ParameterName = "@price", Value = price }
                });
        }

        public long InsertWithdrawal(string uuid, long orderId, string currency, string fromExchange, decimal amount, int processId)
        {
            if (gmail != null)
                EmailHelper.SendSimpleMailAsync(gmail, string.Format("Withdraw Order From {0}: {1}", fromExchange, uuid), string.Format("{0}: {1}", currency, amount));

            return (long)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.InsertWithdrawal", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@uuid", Value = uuid },
                    new SqlParameter { ParameterName = "@orderId", Value = orderId },
                    new SqlParameter { ParameterName = "@currency", Value = currency },
                    new SqlParameter { ParameterName = "@fromExchange", Value = fromExchange },
                    new SqlParameter { ParameterName = "@amount", Value = amount },
                    new SqlParameter { ParameterName = "@pId", Value = processId }
                });
        }

        public void UpdateOrderUuid(long id, string uuid)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateOrderUuid", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@uuid", Value = uuid }
                });
        }

        public void FillOrder(long id, decimal quantity, decimal price, decimal rate, decimal fee = 0)
        {
            if (gmail != null)
                EmailHelper.SendSimpleMailAsync(gmail, string.Format("Order Filled: {0}", id), "");

            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.CloseOrder", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@quantity", Value = quantity },
                    new SqlParameter { ParameterName = "@price", Value = price },
                    new SqlParameter { ParameterName = "@rate", Value = rate },
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

        public void SaveOrderPair(long orderId1, string exchange1, long orderId2, string exchange2)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.SaveOrderPair", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@order1Id", Value = orderId1 },
                    new SqlParameter { ParameterName = "@order1Exchange", Value = exchange1 },
                    new SqlParameter { ParameterName = "@order2Id", Value = orderId2 },
                    new SqlParameter { ParameterName = "@order2Exchange", Value = exchange2 }
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

        public int StartEngineProcess(string baseExchange, string arbExchange, string runType, CurrencyConfig baseCurrency)
        {
            return (int)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.StartEngineProcess", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbExchange },
                    new SqlParameter { ParameterName = "@runType", Value = runType },
                    new SqlParameter { ParameterName = "@baseCurrency", Value = JsonConvert.SerializeObject(baseCurrency) }
                });
        }

        public void EndEngineProcess(int id, string resultStatus, object payload = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.EndEngineProcess", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@result", Value = resultStatus },
                    new SqlParameter { ParameterName = "@meta", Value = (payload == null) ? "" : JsonConvert.SerializeObject(payload)}
                });
        }

        public int GetInvalidOrderCount()
        {
            return (int)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.GetInvalidOrderCount", 15);
        }

        public IEnumerable<DbWithdrawOrder> GetOrdersToWithdraw(string fromExchange, string toExchange, string side, decimal threshold)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetOrdersToWithdrawal", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@fromExchange", Value = fromExchange },
                        new SqlParameter { ParameterName = "@toExchange", Value = toExchange },
                        new SqlParameter { ParameterName = "@side", Value = side },
                        new SqlParameter { ParameterName = "@threshold", Value = threshold }
                }))
            {
                return reader.ToList<DbWithdrawOrder>();
            }
        }

        public void CloseOrders(IEnumerable<long> ids)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.CloseOrders", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@ids", Value = ids.ToDataTable(), TypeName = "dbo.LongCollection" }
                });
        }

        public void InsertArbitragePair(string baseExchange, string arbExchange, string symbol, string baseSymbol, string counterSymbol, string baseCurrency, string marketCurrency)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertArbitragePair", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbExchange },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@baseSymbol", Value = baseSymbol },
                    new SqlParameter { ParameterName = "@counterSymbol", Value = counterSymbol },
                    new SqlParameter { ParameterName = "@baseCurrency", Value = baseCurrency },
                    new SqlParameter { ParameterName = "@marketCurrency", Value = marketCurrency }
                });
        }

        public IEnumerable<DbArbitragePair> GetArbitragePairs(string status, string baseExchange = "", string arbExchange = "")
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetArbitragePairs", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@status", Value = status },
                        new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                        new SqlParameter { ParameterName = "@arbExchange", Value = arbExchange }
                }))
            {
                return reader.ToList<DbArbitragePair>();
            }
        }

        public void UpdateArbitragePair(string baseExchange, string arbExchange, string symbol, bool isTrade = false, bool isError = false, bool isOpportunity = false, object payload = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateArbitragePair", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbExchange },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@isTrade", Value = isTrade },
                    new SqlParameter { ParameterName = "@isError", Value = isError },
                    new SqlParameter { ParameterName = "@isOpportunity", Value = isOpportunity },
                    new SqlParameter { ParameterName = "@meta", Value = (payload == null) ? "" : JsonConvert.SerializeObject(payload)}
                });
        }

        public void UpdateArbitragePairById(int id, bool isTrade = false, bool isError = false, bool isOpportunity = false, bool isFunded = false, object payload = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateArbitragePairById", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@Id", Value = id },
                    new SqlParameter { ParameterName = "@isTrade", Value = isTrade },
                    new SqlParameter { ParameterName = "@isError", Value = isError },
                    new SqlParameter { ParameterName = "@isOpportunity", Value = isOpportunity },
                    new SqlParameter { ParameterName = "@isFunded", Value = isFunded },
                    new SqlParameter { ParameterName = "@meta", Value = (payload == null) ? "" : JsonConvert.SerializeObject(payload)}
                });

        }

        public void InsertArbitrageOpportunity(int pairId, decimal basePrice, decimal arbPrice, decimal spread, decimal threshold)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertOpportunity", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@pairId", Value = pairId },
                    new SqlParameter { ParameterName = "@basePrice", Value = basePrice },
                    new SqlParameter { ParameterName = "@arbPrice", Value = arbPrice},
                    new SqlParameter { ParameterName = "@spread", Value = spread},
                    new SqlParameter { ParameterName = "@threshold", Value = threshold}
                });
        }
    }

}