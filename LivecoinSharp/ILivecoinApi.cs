using System;
using System.Threading.Tasks;
using LivecoinSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace LivecoinSharp
{
    public interface ILivecoinApi
    {
        [Get("/exchange/ticker")]
        Task<IEnumerable<Ticker>> GetTickers();

        [Get("/exchange/ticker?currencyPair={symbol}")]
        Task<Ticker> GetTicker([Path] string symbol);

        [Get("/exchange/order_book?currencyPair={symbol}&groupByPrice=true&depth=25")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);
    }
}