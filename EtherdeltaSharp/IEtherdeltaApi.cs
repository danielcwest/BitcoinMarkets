using System;
using System.Threading.Tasks;
using EtherdeltaSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace EtherdeltaSharp
{
    public interface IEtherdeltaApi
    {
        [Get("/returnTicker")]
        Task<Dictionary<string, Ticker>> GetTickers();
    }
}