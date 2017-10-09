using System;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;
using System.Threading.Tasks;
using System.Text;
using LiquiSharp.Models;

namespace LiquiSharp
{
    public class Liqui
    {
        ILiquiApi _liqui;

        public Liqui()
        {
            _liqui = RestClient.For<ILiquiApi>("https://api.liqui.io");
        }

        public async Task<IEnumerable<object>> Info()
        {
            var summaries = await _liqui.Info();
            return summaries.pairs.Keys;
        }
        public async Task<Dictionary<string, Ticker>> Pairs()
        {
            var pairs = await _liqui.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _liqui.GetTickers(sb.ToString());

            return summaries;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var pairs = await _liqui.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _liqui.GetTickers(sb.ToString());

            return summaries.Select(s => new Market(s.Key, s.Value));
        }
    }
}
