using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core.Models;
using Core.Config;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class ConfigController : Controller
    {
        IConfiguration _iconfiguration;
        public ConfigController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        // GET api/config
        [HttpGet]
        public IEnumerable<ConfigGroup> Get()
        {
            return new List<ConfigGroup>() { new ConfigGroup(){
                name = "Group 1",
                description = "Group 1",
                symbols = new string[] { "BTC", "ETH", "XRP"}
            }, new ConfigGroup(){
                name = "Group 2",
                description = "Group 2",
                symbols = new string[] { "BCH", "ETC", "XMR"}
            }};
        }
    }
}
