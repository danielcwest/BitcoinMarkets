using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BittrexSharp;
using BittrexSharp.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class BittrexController : Controller
    {
        IConfiguration _iconfiguration;
        Bittrex _bittrex;
        public BittrexController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _bittrex = new Bittrex(_iconfiguration.GetValue<string>("Bittrex:ApiKey"), _iconfiguration.GetValue<string>("Bittrex:Secret"));
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _bittrex.MarketSummaries();
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _bittrex.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _bittrex.OrderBook(symbol);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _bittrex.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }
    }
}