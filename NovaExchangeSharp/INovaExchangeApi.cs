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
    }
}