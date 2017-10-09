using System;
using System.Threading.Tasks;
using CoinExchangeSharp.Models;
using RestEase;
using System.Collections.Generic;
using BMCore.Models;
using System.Linq;

namespace CoinExchangeSharp
{

    public class CoinExchange
    {
        ICoinExchangeApi _coin;

        public CoinExchange()
        {
            _coin = RestClient.For<ICoinExchangeApi>("https://www.coinexchange.io");

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

    }
}