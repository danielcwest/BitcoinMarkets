using System;
using System.Threading.Tasks;
using BitzSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Models;
using System.Linq;

namespace BitzSharp
{

    public class Bitz
    {
        IBitzApi _bitz;

        public Bitz()
        {
            _bitz = RestClient.For<IBitzApi>("https://www.bit-z.com");
        }

        public async Task<Dictionary<string, Ticker>> Tickers()
        {
            var tickers = await _bitz.GetTickers();
            return tickers.data;
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _bitz.GetTickers();
            return summaries.data.Where(s => s.Key.EndsWith("_btc") || s.Key.EndsWith("_eth")).Select(s => new Market(s.Key, s.Value));
        }
    }
}