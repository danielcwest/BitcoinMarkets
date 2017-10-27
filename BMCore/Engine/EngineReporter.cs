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
    }
}