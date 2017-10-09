using System;
using System.Threading.Tasks;
using NovaExchangeSharp.Models;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;

namespace NovaExchangeSharp
{
    public class Nova
    {
        INovaExchangeApi _nova;

        public Nova()
        {
            _nova = RestClient.For<INovaExchangeApi>("https://novaexchange.com");
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var summary = await _nova.GetTickers();
            return summary.markets.Where(s => s.basecurrency.Equals("ETH") || s.basecurrency.Equals("BTC")).Select(s => new Market(s));
        }
    }
}