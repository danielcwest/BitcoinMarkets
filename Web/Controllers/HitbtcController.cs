using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HitbtcSharp;
using HitbtcSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core.Models;
using RestEase;
using Core.Contracts;
using Core.Config;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class HitbtcController : Controller
    {
        IConfiguration _iconfiguration;
        Hitbtc _hitbtc;
        public HitbtcController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Hitbtc");
            _hitbtc = new Hitbtc(config);
        }

    }
}
