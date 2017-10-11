using System;
using System.Threading.Tasks;
using BitzSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace BitzSharp
{
    public interface IBitzApi
    {
        [Get("/api_v1/tickerall")]
        Task<BitzResponse<Dictionary<string, Ticker>>> GetTickers();

        [Get("/api_v1/ticker?coin={symbol}")]
        Task<BitzResponse<Ticker>> GetTicker([Path] string symbol);

        [Get("/api_v1/depth?coin={symbol}")]
        Task<BitzResponse<Depth>> GetOrderBook([Path] string symbol);
    }
}