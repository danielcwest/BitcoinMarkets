using System;
using System.Threading.Tasks;
using LiquiSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace LiquiSharp
{
    public interface ILiquiApi
    {
        [Get("/api/3/info")]
        Task<Info> Info();

        [Get("/api/3/ticker/{tickers}")]
        Task<Dictionary<string, Ticker>> GetTickers([Path] string tickers);

        [Get("/api/3/depth/{symbol}?limit=25")]
        Task<Dictionary<string, OrderbookResponse>> GetOrderBook([Path] string symbol);
    }
}