using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BitzSharp;
using BitzSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Contracts;
using RestEase;
using BMCore.Models;
using BMCore.Config;

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
            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Bitz");
            _bitz = new Bitz(config);
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

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _bitz.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _bitz.OrderBook(symbol);
        }

    }
}