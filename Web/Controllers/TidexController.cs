using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TidexSharp;
using TidexSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Contracts;
using RestEase;
using System.Text;
using BMCore.Models;
using BMCore.Config;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class TidexController : Controller
    {
        IConfiguration _iconfiguration;
        Tidex _tidex;
        public TidexController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Tidex");
            _tidex = new Tidex(config);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _tidex.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _tidex.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _tidex.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _tidex.OrderBook(symbol);
        }

    }
}