using System;
using System.Threading.Tasks;
using NovaExchangeSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace NovaExchangeSharp
{
    public interface INovaExchangeApi
    {
        [Get("/remote/v2/markets")]
        Task<NovaResponse> GetTickers();

        [Get("/remote/v2/market/info/{symbol}/")]
        Task<NovaResponse> GetTicker([Path] string symbol);

        [Get("/remote/v2/market/openorders/{symbol}/BOTH/")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);
    }
}