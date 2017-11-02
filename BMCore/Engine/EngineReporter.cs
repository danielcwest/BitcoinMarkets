using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;
using BMCore.Models;

namespace BMCore.Engine
{
    public class EngineReporter
    {
        IExchange baseExchange;
        IExchange counterExchange;
        IDbService dbService;

        IEnumerable<ICurrencyBalance> baseExchangeBalances;
        IEnumerable<ICurrencyBalance> counterExchangeBalances;


        public EngineReporter(IExchange baseExchange, IExchange counterExchange, IDbService dbService)
        {
            this.baseExchange = baseExchange;
            this.counterExchange = counterExchange;
            this.dbService = dbService;
        }

        public async Task TakeBalanceSnapshot(int pId = -1)
        {
            await RefreshBalances();

            var baseTasks = baseExchangeBalances.AsParallel().Select(async b => await TakeBalanceSnapshot(baseExchange, b, pId));                            
            var counterTasks = counterExchangeBalances.AsParallel().Select(async b => await TakeBalanceSnapshot(counterExchange, b, pId));
           
            await Task.WhenAll(baseTasks);
            await Task.WhenAll(counterTasks);
        }

        private async Task TakeBalanceSnapshot(IExchange exchange, ICurrencyBalance balance, int pId = -1)
        {
            decimal price = 0.0m;
            string symbol = "";
            try
            {
                //Check the current price of Market Currencies
                if (balance.Currency.ToUpperInvariant() != "BTC" && !balance.Currency.ToUpperInvariant().Contains("USD"))
                {
                    symbol = string.Format("{0}BTC", balance.Currency);

                }//Check current price of BTC
                else if (balance.Currency.ToUpperInvariant() == "BTC")
                {
                    symbol = "BTCUSD";
                }

                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    var ticker = await exchange.Ticker(symbol);
                    price = ticker.Last;
                    dbService.InsertBalanceSnapshot(balance.Currency, exchange.Name, balance.Available, balance.Held, price, pId);
                }
            }
            catch (RestEase.ApiException ae)
            {
                //Ignore RestEase bad request for now
                if (ae.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, symbol, "TakeBalanceSnapshot", ae, pId);
                }
            }
            catch (Exception e)
            {
                dbService.LogError(this.baseExchange.Name, this.counterExchange.Name, symbol, "TakeBalanceSnapshot", e, pId);
            }
            finally
            {
                Console.WriteLine("{0} {1} Balance: {2}, Held: {3}", exchange.Name, balance.Currency, balance.Available, balance.Held);
            }
        }

        private async Task RefreshBalances()
        {
            var baseBalances = await this.baseExchange.GetBalances();
            this.baseExchangeBalances = baseBalances.Where(b => b.Available > 0);

            var arbBalances = await this.counterExchange.GetBalances();
            this.counterExchangeBalances = arbBalances.Where(b => b.Available > 0);
        }

        #region Static Helpers
        public static async Task GenerateEmailReport(BMDbService dbService, Dictionary<string, IExchange> exchanges, GmailConfig gmail)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("********** Arbitrage Report **********");
            sb.AppendLine("");
            sb.AppendLine("");

            var recentOpps = dbService.GetRecentOpportunities().GroupBy(g => new { g.BaseExchange, g.CounterExchange, g.Symbol }).Select(g => new
            {
                BaseExchange = g.Key.BaseExchange,
                CounterExchange = g.Key.CounterExchange,
                Symbol = g.Key.Symbol,
                Opportunities = g.ToList()
            });


            sb.AppendLine("Opportunities");

            foreach (var opp in recentOpps)
            {
                sb.AppendLine(string.Format("{0} {1} {2} Spread: {3:P2} TradeThreshold: {4} SpreadThreshold: {5}, Count: {6}", opp.BaseExchange, opp.CounterExchange, opp.Symbol, opp.Opportunities.Average(o => o.Spread), opp.Opportunities.FirstOrDefault().TradeThreshold, opp.Opportunities.FirstOrDefault().SpreadThreshold, opp.Opportunities.Count));
            }

            sb.AppendLine("");
            sb.AppendLine("");

            var recentTxs = dbService.GetRecentTransactions();

            sb.AppendLine("Trades");

            foreach (var tx in recentTxs)
            {
                if (!string.IsNullOrWhiteSpace(tx.BaseTxId) && !string.IsNullOrWhiteSpace(tx.CounterTxId))
                    sb.AppendLine(string.Format("{0} {1} {2} Commission: {3}", tx.BaseExchange, tx.CounterExchange, tx.Symbol, tx.Commission));
                else
                    sb.AppendLine(string.Format("{0} {1} {2} Commission: {3}", tx.BaseExchange, tx.CounterExchange, tx.Symbol, "PENDING"));
            }

            sb.AppendLine("");
            sb.AppendLine("");

            var groups = dbService.GetArbitragePairs("").GroupBy(g => new { g.BaseExchange, g.CounterExchange }).Select(g => new ArbitrageGroup()
            {
                BaseExchange = g.Key.BaseExchange,
                CounterExchange = g.Key.CounterExchange,
                Markets = g.Select(m => new ArbitragePair(m))
            });

            sb.AppendLine("Exchanges");

            foreach (var group in groups)
            {
                sb.AppendLine("");
                sb.AppendLine(string.Format("{0} {1}", group.BaseExchange, group.CounterExchange));

                var tradePairs = dbService.GetArbitragePairs("trade", group.BaseExchange, group.CounterExchange);
                sb.AppendLine(string.Format("Trading Pairs: {0}", tradePairs.Count()));

                var logPairs = dbService.GetArbitragePairs("log", group.BaseExchange, group.CounterExchange);
                sb.AppendLine(string.Format("Log Pairs: {0}", logPairs.Count()));

                var errPairs = dbService.GetArbitragePairs("error", group.BaseExchange, group.CounterExchange);
                sb.AppendLine(string.Format("Error Pairs: {0}", errPairs.Count()));
                sb.AppendLine("");
            }

            await EmailHelper.SendSimpleMailAsync(gmail, "Arbitrage Report", sb.ToString());

        }

        #endregion
    }
}