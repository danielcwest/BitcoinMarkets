using System;
using System.Threading.Tasks;
using TidexSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Models;
using System.Linq;
using System.Text;

namespace TidexSharp
{
    public class Tidex
    {
        ITidexApi _tidex;
        public Tidex()
        {
            _tidex = RestClient.For<ITidexApi>("https://api.tidex.com");
        }

        public async Task<IEnumerable<object>> Info()
        {
            var summaries = await _tidex.Info();
            return summaries.pairs.Keys;
        }

        public async Task<Dictionary<string, Ticker>> Pairs()
        {
            var pairs = await _tidex.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _tidex.GetTickers(sb.ToString());

            return summaries;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var pairs = await _tidex.Info();

            var sb = new StringBuilder();
            foreach (var pair in pairs.pairs.Keys.Where(k => !k.Contains("usdt")))
            {
                sb.Append(string.Format("{0}-", pair));
            }
            sb.Length--;

            var summaries = await _tidex.GetTickers(sb.ToString());

            return summaries.Select(s => new Market(s.Key, s.Value));
        }
    }
}