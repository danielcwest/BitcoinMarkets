using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EtherdeltaSharp;
using EtherdeltaSharp.Models;
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
    public class EtherdeltaController : Controller
    {
        IConfiguration _iconfiguration;
        Etherdelta _etherdelta;
        public EtherdeltaController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Etherdelta");
            _etherdelta = new Etherdelta(config);
        }

        [HttpGet]
        public async Task<Dictionary<string, Ticker>> Tickers()
        {
            return await _etherdelta.Tickers();
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _etherdelta.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _etherdelta.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

    }
}