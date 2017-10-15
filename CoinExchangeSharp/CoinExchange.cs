using System;
using System.Threading.Tasks;
using CoinExchangeSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Contracts;
using System.Linq;
using BMCore.Models;

namespace CoinExchangeSharp
{

    public class CoinExchange
    {
        ICoinExchangeApi _coin;
        string Name;
        decimal Fee;

        public CoinExchange(ConfigExchange config)
        {
            _coin = RestClient.For<ICoinExchangeApi>("https://www.coinexchange.io");
            Name = config.Name;
            Fee = config.Fee;
        }

        public async Task<CoinResponse<CoinExchangeSharp.Models.CoinXMarket>> Markets()
        {
            return await _coin.GetMarkets();
        }

        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            var markets = await _coin.GetMarkets();

            var marketDic = markets.result.Where(m => m.BaseCurrencyCode == "BTC" || m.BaseCurrencyCode == "ETH").ToDictionary(m => m.MarketID);

            var tickers = await _coin.GetTickers();

            var tickersDic = tickers.result.ToDictionary(t => t.MarketID);

            return marketDic.Select(m => new Market(m.Value, tickersDic[m.Key]));
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