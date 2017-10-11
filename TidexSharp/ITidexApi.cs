using System;
using System.Threading.Tasks;
using TidexSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace TidexSharp
{
    public interface ITidexApi
    {
        [Get("/api/3/info")]
        Task<Info> Info();

        [Get("/api/3/ticker/{tickers}")]
        Task<Dictionary<string, Ticker>> GetTickers([Path] string tickers);

        [Get("/api/3/depth/{symbol}?limit=25")]
        Task<Dictionary<string, OrderbookResponse>> GetOrderBook([Path] string symbol);
    }
}