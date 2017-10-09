using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BitzSharp;
using BitzSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class BitzController : Controller
    {
        IConfiguration _iconfiguration;
        Bitz _bitz;
        public BitzController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _bitz = new Bitz();
        }

        [HttpGet]
        public async Task<Dictionary<string, Ticker>> Tickers()
        {
            return await _bitz.Tickers();
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _bitz.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _bitz.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

    }
}