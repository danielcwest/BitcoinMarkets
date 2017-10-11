using System;
using System.Threading.Tasks;
using EtherdeltaSharp.Models;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;

namespace EtherdeltaSharp
{

    public class Etherdelta
    {
        IEtherdeltaApi _etherdelta;

        public Etherdelta(string apiKey, string apiSecret)
        {
            _etherdelta = RestClient.For<IEtherdeltaApi>("https://api.etherdelta.com");

        }


        public async Task<Dictionary<string, Ticker>> Tickers()
        {
            return await _etherdelta.GetTickers();
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summaries = await _etherdelta.GetTickers();
            return summaries.Where(s => s.Key.StartsWith("ETH_")).Select(s => new Market(s.Key, s.Value));
        }
    }
}