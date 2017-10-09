using System;
using System.Threading.Tasks;
using LivecoinSharp.Models;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;

namespace LivecoinSharp
{
    public class Livecoin
    {

        ILivecoinApi _livecoin;

        public Livecoin()
        {
            _livecoin = RestClient.For<ILivecoinApi>("https://api.livecoin.net/");
        }

        public async Task<IEnumerable<Ticker>> Tickers()
        {
            return await _livecoin.GetTickers();
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _livecoin.GetTickers();
            return summaries.Where(s => s.symbol.EndsWith("/BTC") || s.symbol.EndsWith("/ETH")).Select(s => new Market(s));
        }
    }
}