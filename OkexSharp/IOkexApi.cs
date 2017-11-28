using OkexSharp.Models;
using RestEase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OkexSharp
{
    public interface IOkexApi
    {
        [Get("/api/v1/depth.do?symbol={symbol}&size=25")]
        Task<OrderBookResponse> GetOrderBook([Path] string symbol);
    }
}
