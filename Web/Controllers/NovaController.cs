using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NovaExchangeSharp;
using NovaExchangeSharp.Models;
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
    public class NovaController : Controller
    {
        IConfiguration _iconfiguration;
        Nova _nova;
        public NovaController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Nova");
            _nova = new Nova(config);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _nova.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _nova.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _nova.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _nova.OrderBook(symbol);
        }

    }
}