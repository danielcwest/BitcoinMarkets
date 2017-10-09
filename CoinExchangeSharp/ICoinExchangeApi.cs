using System;
using System.Threading.Tasks;
using CoinExchangeSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace CoinExchangeSharp
{
    public interface ICoinExchangeApi
    {
        [Get("/api/v1/getmarkets")]
        Task<CoinResponse<CoinXMarket>> GetMarkets();

        [Get("/api/v1/getmarketsummaries")]
        Task<CoinResponse<Ticker>> GetTickers();
    }
}