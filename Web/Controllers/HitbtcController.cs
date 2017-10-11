using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HitbtcSharp;
using HitbtcSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class HitbtcController : Controller
    {
        IConfiguration _iconfiguration;
        Hitbtc _hitbtc;
        public HitbtcController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _hitbtc = new Hitbtc("", "");
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _hitbtc.MarketSummaries();
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _hitbtc.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _hitbtc.OrderBook(symbol);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _hitbtc.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

    }
}