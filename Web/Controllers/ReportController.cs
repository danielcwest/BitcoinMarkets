using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BMCore.DbService;
using BMCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Config;

namespace Web.Controllers
{

    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class ReportController : Controller
    {

        IConfiguration _iconfiguration;

        IDbService dbService;

        public ReportController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ExchangeConfig>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            dbService = new BMDbService(_iconfiguration.GetValue<string>("SqlConnectionString"));

        }

        [HttpGet]
        public IEnumerable<DbOrder> Orders()
        {
            return dbService.GetOrders();
        }

        [HttpGet]
        public IEnumerable<DbTradeLog> Trades()
        {
            return dbService.GetTrades();
        }

        [HttpGet]
        public IEnumerable<DbWithdrawal> Withdrawals()
        {
            return dbService.GetWithdrawals();
        }
    }
}