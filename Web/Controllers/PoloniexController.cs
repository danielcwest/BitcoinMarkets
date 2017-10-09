using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PoloniexSharp;
using PoloniexSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class PoloniexController : Controller
    {
        IConfiguration _iconfiguration;
        Poloniex _poloniex;
        public PoloniexController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _poloniex = new Poloniex();
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _poloniex.MarketSummaries();
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _poloniex.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _poloniex.OrderBook(symbol);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _poloniex.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }
    }
}