using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TidexSharp;
using TidexSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;
using System.Text;

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
            _tidex = new Tidex();
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

    }
}