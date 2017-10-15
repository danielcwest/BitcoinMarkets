using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LivecoinSharp;
using LivecoinSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Contracts;
using RestEase;
using BMCore.Models;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class LivecoinController : Controller
    {
        IConfiguration _iconfiguration;
        Livecoin _livecoin;
        public LivecoinController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ConfigExchange>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Livecoin");
            _livecoin = new Livecoin(config);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _livecoin.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _livecoin.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _livecoin.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _livecoin.OrderBook(symbol);
        }
    }
}