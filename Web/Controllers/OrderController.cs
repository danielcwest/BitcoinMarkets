using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.DbService;
using Microsoft.Extensions.Configuration;
using NLog;
using Core.Config;

namespace Web.Controllers
{
	[Route("api/[controller]/[action]")]
	[Produces("application/json")]
	public class OrderController : Controller
    {
		private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

		IConfiguration _iconfiguration;

		IDbService dbService;

		public OrderController(IConfiguration iconfiguration)
		{
			_iconfiguration = iconfiguration;
			var exchanges = new List<ExchangeConfig>();
			_iconfiguration.GetSection("Exchanges").Bind(exchanges);
			dbService = new BMDbService(_iconfiguration.GetValue<string>("LocalSqlConnectionString"));
		}

		[HttpGet]
		public IEnumerable<DbMakerOrder> Get(int pairId)
		{
			return dbService.GetMakerOrdersByStatus("complete", pairId);
		}
	}
}
