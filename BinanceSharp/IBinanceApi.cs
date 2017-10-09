using System;
using System.Threading.Tasks;
using BinanceSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace BinanceSharp
{
    public interface IBinanceApi
    {
        [Get("/api/v1/ticker/allPrices")]
        Task<IEnumerable<PriceTicker>> GetPrices();

        [Get("/api/v1/ticker/allBookTickers")]
        Task<IEnumerable<BookTicker>> GetBooks();

        /*
                {
                    "lastUpdateId": 1027024,
                    "bids": [
                        [
                        "4.00000000",     // PRICE
                        "431.00000000",   // QTY
                        []                // Can be ignored
                        ]
                    ],
                    "asks": [
                        [
                        "4.00000200",
                        "12.00000000",
                        []
                        ]
                    ]
                }
                 */
        [Get("/api/v1/depth?symbol={symbol}")]
        Task<OrderBookResponse> GetOrderBook([Path] string symbol);

        [Get("/api/v1/ticker/24hr?symbol={symbol}")]
        Task<Ticker> GetTicker([Path] string symbol);
    }
}