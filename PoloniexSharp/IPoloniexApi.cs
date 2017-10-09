using System;
using System.Threading.Tasks;
using PoloniexSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace PoloniexSharp
{
    public interface IPoloniexApi
    {
        [Get("/public?command=returnTicker")]
        Task<Dictionary<string, Ticker>> GetTickers();

        [Get("/public?command=returnOrderBook&currencyPair={symbol}&depth=25")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);

    }
}