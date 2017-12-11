using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.DbService;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core.Config;
using Core.Contracts;
using Newtonsoft.Json;
using NLog;

namespace Web.Controllers
{

	[Route("api/[controller]/[action]")]
	[Produces("application/json")]
	public class ArbitrageController : Controller
	{
		private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

		IConfiguration _iconfiguration;

		IDbService dbService;

		public ArbitrageController(IConfiguration iconfiguration)
		{
			_iconfiguration = iconfiguration;
			var exchanges = new List<ExchangeConfig>();
			_iconfiguration.GetSection("Exchanges").Bind(exchanges);
			dbService = new BMDbService(_iconfiguration.GetValue<string>("LocalSqlConnectionString"));
		}

		[HttpGet]
		public IEnumerable<DbArbitragePair> Get()
		{
			return dbService.GetArbitragePairs("");
		}

		[HttpGet]
		public DbArbitragePair GetPair(int id)
		{
			return dbService.GetArbitragePairs("").Single(p => p.Id == id);
		}

		[HttpPost]
		public void Save([FromBody] ArbitragePair pair)
		{
			dbService.SaveArbitragePair(pair);
		}

		[HttpGet]
		public IEnumerable<DbHeroStat> GetHeroStats(string interval)
		{
			int hours = 1;
			if (interval == "4h")
				hours = 4;
			else if(interval == "12h")			
				hours = 12;
			else if (interval == "24h")
				hours = 24;
			else if(interval == "7d")		
				hours = 168;
			
			return dbService.GetHeroStats(hours);
		}
	}
}
