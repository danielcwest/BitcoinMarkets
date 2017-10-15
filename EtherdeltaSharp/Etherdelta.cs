using System;
using System.Threading.Tasks;
using EtherdeltaSharp.Models;
using RestEase;
using System.Collections.Generic;
using System.Linq;
using BMCore.Contracts;
using BMCore.Models;

namespace EtherdeltaSharp
{

    public class Etherdelta
    {
        IEtherdeltaApi _etherdelta;
        string Name;
        decimal Fee;

        public Etherdelta(ConfigExchange config)
        {
            _etherdelta = RestClient.For<IEtherdeltaApi>("https://api.etherdelta.com");
            Name = config.Name;
            Fee = config.Fee;

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

        public decimal GetFee()
        {
            return Fee;
        }

        public string GetExchangeName()
        {
            return Name;
        }
    }
}