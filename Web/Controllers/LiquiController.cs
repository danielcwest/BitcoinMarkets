using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LiquiSharp;
using LiquiSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Contracts;
using RestEase;
using System.Text;
using BMCore.Models;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class LiquiController : Controller
    {
        IConfiguration _iconfiguration;
        Liqui _liqui;
        public LiquiController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ConfigExchange>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Liqui");
            _liqui = new Liqui(config);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _liqui.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _liqui.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _liqui.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _liqui.OrderBook(symbol);
        }
    }
}