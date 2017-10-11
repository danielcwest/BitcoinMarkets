using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BinanceSharp;
using BinanceSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class BinanceController : Controller
    {
        IConfiguration _iconfiguration;
        Binance _binance;

        public BinanceController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _binance = new Binance("", "");
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _binance.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _binance.MarketSummaries();
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _binance.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _binance.OrderBook(symbol);
        }

    }
}