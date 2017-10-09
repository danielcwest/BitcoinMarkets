using System;
using HitbtcSharp.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestEase;
using System.Collections.Generic;

namespace HitbtcSharp
{
    public interface IHitbtcApi
    {
        [Get("/api/1/public/symbols")]
        Task<Symbols> GetSymbols();

        [Get("/api/1/public/ticker")]
        Task<Dictionary<string, Ticker>> GetTickers();

        [Get("/api/1/public/{symbol}/ticker")]
        Task<Ticker> GetTicker([Path] string symbol);

        [Get("/api/1/public/{symbol}/orderbook?format_price=number&format_amount=number")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);
    }
}