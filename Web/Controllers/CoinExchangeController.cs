using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoinExchangeSharp;
using CoinExchangeSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class CoinExchangeController : Controller
    {
        IConfiguration _iconfiguration;
        CoinExchange _coin;
        public CoinExchangeController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _coin = new CoinExchange("", "");
        }

        [HttpGet]
        public async Task<CoinResponse<CoinExchangeSharp.Models.CoinXMarket>> Markets()
        {
            return await _coin.Markets();
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _coin.MarketSummaries();
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _coin.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

    }
}