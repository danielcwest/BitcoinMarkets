using System;
using System.Threading.Tasks;
using BitzSharp.Models;
using RestEase;
using System.Collections.Generic;

namespace BitzSharp
{
    public interface IBitzApi
    {
        [Get("/api_v1/tickerall")]
        Task<BitzResponse> GetTickers();
    }
}