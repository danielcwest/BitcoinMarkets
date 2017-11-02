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
using BMCore.Contracts;
using BMCore.Config;

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

            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Bittrex");
            _bittrex = new Bittrex(config);
        }

    }
}
