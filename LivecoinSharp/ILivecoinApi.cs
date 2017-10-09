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
    }
}