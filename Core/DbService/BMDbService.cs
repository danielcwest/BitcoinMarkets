using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core.Config;
using RestEase;
using Core.Models;
using Core.Contracts;

namespace Core.DbService
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
                LogError(baseX, arbX, symbol, method, message, stackTrace);

                if (gmail != null)
                    EmailHelper.SendSimpleMailAsync(gmail, string.Format("Error: {0}", ex.Message), string.Format("{0} - {1} {2} {3}", baseX, arbX, Environment.NewLine, stackTrace));
            }
            catch (Exception)
            {
                //fail silently
            }
        }

        public void LogError(string baseX, string arbX, string symbol, string method, ApiException aex, int processId = -1)
        {
            string message = aex.Message;
            string content = aex.Content;
           
            try
            {
                LogError(baseX, arbX, symbol, method, message, content);
            }
            catch (Exception)
            {
                //fail silently
            }
        }

        public void LogError(string baseX, string arbX, string symbol, string method, string message, string content, int processId = -1)
        {
                DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertError", 15,
                                new SqlParameter[]
                                {
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseX },
                    new SqlParameter { ParameterName = "@arbExchange", Value = arbX },
                    new SqlParameter { ParameterName = "@symbol", Value = symbol },
                    new SqlParameter { ParameterName = "@method", Value = method },
                    new SqlParameter { ParameterName = "@message", Value = message},
                    new SqlParameter { ParameterName = "@exception", Value = content},
                    new SqlParameter { ParameterName = "@processId", Value = processId}
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

        public IEnumerable<DbArbitragePair> GetArbitragePairs(string type, string baseExchange = "", string arbExchange = "")
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetArbitragePairs", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@type", Value = type },
                        new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                        new SqlParameter { ParameterName = "@arbExchange", Value = arbExchange }
                }))
            {
                return reader.ToList<DbArbitragePair>();
            }
        }

        public void SaveArbitragePair(IArbitragePairDTO dto)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.SaveArbitragePair", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = dto.Id },
                    new SqlParameter { ParameterName = "@status", Value = dto.Status },
                    new SqlParameter { ParameterName = "@type", Value = dto.Type },
                    new SqlParameter { ParameterName = "@tradeThreshold", Value = dto.TradeThreshold },
                    new SqlParameter { ParameterName = "@increment", Value = dto.Increment },
                    new SqlParameter { ParameterName = "@tickSize", Value = dto.TickSize },
                    new SqlParameter { ParameterName = "@askSpread", Value = dto.AskSpread },
                    new SqlParameter { ParameterName = "@bidSpread", Value = dto.BidSpread }
                });
        }

        public int InsertTransaction(int pairId, string type)
        {
            return (int)DbServiceHelper.ExecuteScalar(sqlConnectionString, "dbo.InsertTransaction", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@pairId", Value = pairId },
                    new SqlParameter { ParameterName = "@type", Value = type }
                });
        }

        public void UpdateTransactionOrderUuid(int id, string baseUuid, string counterUuid, object payload = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateTransactionOrderUuid", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@baseUuid", Value = baseUuid },
                    new SqlParameter { ParameterName = "@counterUuid", Value = counterUuid},
                    new SqlParameter { ParameterName = "@meta", Value = (payload == null) ? "" : JsonConvert.SerializeObject(payload)}
                });
        }

        public void UpdateTransactionWithdrawalUuid(int id, string baseUuid, string counterUuid, decimal commission)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateTransactionWithdrawalUuid", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@baseUuid", Value = baseUuid },
                    new SqlParameter { ParameterName = "@counterUuid", Value = counterUuid},
                    new SqlParameter { ParameterName = "@commission", Value = commission}
                });
        }

        public void CloseTransaction(int id, string baseTxId, string counterTxId)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.CloseTransaction", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@baseTxId", Value = baseTxId },
                    new SqlParameter { ParameterName = "@counterTxId", Value = counterTxId}
                });
        }

        public void UpdateTransactionStatus(int id, string status, object payload = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateTransactionStatus", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@status", Value = status },
                    new SqlParameter { ParameterName = "@meta", Value = (payload == null) ? "" : JsonConvert.SerializeObject(payload)}
                });
        }

        public IEnumerable<DbTransaction> GetTransactions(string status, int pairId = 0)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetTransactions", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@status", Value = status },
                        new SqlParameter { ParameterName = "@pairId", Value = pairId }
                }))
            {
                return reader.ToList<DbTransaction>();
            }
        }

        public IEnumerable<DbReportTransaction> GetRecentTransactions(int hours = 1)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetRecentTransactions", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@hours", Value = hours }
                }))
            {
                return reader.ToList<DbReportTransaction>();
            }
        }

        public IEnumerable<DbOpportunity> GetRecentOpportunities(int hours = 1)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetRecentOpportunities", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@hours", Value = hours }
                }))
            {
                return reader.ToList<DbOpportunity>();
            }
        }

        public void InsertMakerOrder(int pairId, string type, string baseUuid, string counterUuid)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertMakerOrder", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@pairId", Value = pairId },
                    new SqlParameter { ParameterName = "@type", Value = type },
                    new SqlParameter { ParameterName = "@baseUuid", Value = baseUuid },
                    new SqlParameter { ParameterName = "@counterUuid", Value = counterUuid }
                });
        }

        public void UpdateOrderUuid(int id, string baseUuid, string counterUuid = "", decimal counterRate = 0, string status = "")
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateOrderUuid", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@baseUuid", Value = baseUuid },
                    new SqlParameter { ParameterName = "@counterUuid", Value = counterUuid},
                    new SqlParameter { ParameterName = "@counterRate", Value = counterRate},
                    new SqlParameter { ParameterName = "@status", Value = status}
                });
        }

        public void UpdateOrderStatus(int id, string status, decimal baseQuantityFilled = 0, decimal counterQuantityFilled = 0)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateOrderStatus", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@status", Value = status },
                    new SqlParameter { ParameterName = "@baseQuantityFilled", Value = baseQuantityFilled},
                    new SqlParameter { ParameterName = "@counterQuantityFilled", Value = counterQuantityFilled}
                });
        }

        public IEnumerable<DbMakerOrder> GetMakerOrdersByStatus(string status, int pairId = 0)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetMakerOrdersByStatus", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@status", Value = status },
                        new SqlParameter { ParameterName = "@pairId", Value = pairId }
                }))
            {
                return reader.ToList<DbMakerOrder>();
            }
        }

        public void InsertBalanceSnapshot(string currency, string baseExchange, string counterExchange, decimal total, decimal price, int processId = 0)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.InsertBalanceSnapshot", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@currency", Value = currency },
                    new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                    new SqlParameter { ParameterName = "@counterExchange", Value = counterExchange },
                    new SqlParameter { ParameterName = "@total", Value = total},
                    new SqlParameter { ParameterName = "@price", Value = price},
                    new SqlParameter { ParameterName = "@processId", Value = processId}
                });
        }

        public IEnumerable<DbBalance> GetBalanceSnapshots(string currency = "", string baseExchange = "", string counterExchange = "", DateTime? fromDate = null, int processId = 0)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetBalances", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@currency", Value = currency },
                        new SqlParameter { ParameterName = "@baseExchange", Value = baseExchange },
                        new SqlParameter { ParameterName = "@counterExchange", Value = counterExchange },   
                        new SqlParameter { ParameterName = "@fromDate", Value = fromDate },
                        new SqlParameter { ParameterName = "@processId", Value = processId }
                }))
            {
                return reader.ToList<DbBalance>();
            }
        }

        public void UpdateOrder(int id, string baseUuid = "", string counterUuid = "", decimal baseRate = 0, decimal counterRate = 0, string status = "", decimal baseQuantityFilled = 0, decimal counterQuantityFilled = 0, decimal baseCost = 0m, decimal counterCost = 0m, decimal commission = 0m, object payload = null)
        {
            DbServiceHelper.ExecuteNonQuery(sqlConnectionString, "dbo.UpdateOrder", 15,
                new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@id", Value = id },
                    new SqlParameter { ParameterName = "@baseUuid", Value = baseUuid },
                    new SqlParameter { ParameterName = "@counterUuid", Value = counterUuid },
                    new SqlParameter { ParameterName = "@baseRate", Value = baseRate },
                    new SqlParameter { ParameterName = "@counterRate", Value = counterRate },
                    new SqlParameter { ParameterName = "@status", Value = status },
                    new SqlParameter { ParameterName = "@baseQuantityFilled", Value = baseQuantityFilled },
                    new SqlParameter { ParameterName = "@counterQuantityFilled", Value = counterQuantityFilled },
                    new SqlParameter { ParameterName = "@baseCost", Value = baseCost },
                    new SqlParameter { ParameterName = "@counterCost", Value = counterCost },
                    new SqlParameter { ParameterName = "@commission", Value = commission },
                    new SqlParameter { ParameterName = "@meta", Value = (payload == null) ? "" : JsonConvert.SerializeObject(payload) }
                });
        }

        public IEnumerable<DbHeroStat> GetHeroStats(int hours)
        {
            using (var reader = DbServiceHelper.ExecuteQuery(sqlConnectionString, "dbo.GetHeroStats", 15,
                new SqlParameter[]
                {
                        new SqlParameter { ParameterName = "@hours", Value = hours }
                }))
            {
                return reader.ToList<DbHeroStat>();
            }
        }
    }
}

