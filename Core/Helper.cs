using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Newtonsoft.Json;
using NLog;

namespace Core
{
    public class Helper
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static List<OrderBookEntry> SumOrderEntries(List<OrderBookEntry> entries)
        {
            for (var i = 0; i < entries.Count; i++)
            {
                if (i == 0)
                {
                    entries[0].sum = entries[0].quantity;
                    entries[0].sumBase = entries[0].quantity * entries[0].price;
                }
                else if(entries[i] != null)
                {
                    entries[i].sum = entries[i - 1].sum + entries[i].quantity;
                    entries[i].sumBase = entries[i - 1].sumBase + (entries[i].quantity * entries[i].price);
                }
            }
            return entries;
        }

        public static decimal RoundQuantity(ArbitragePair pair, decimal quantity)
        {
            return RoundToIncrement(quantity, pair.Increment);
        }

        public static decimal RoundPrice(ArbitragePair pair, decimal price)
        {
            return RoundToIncrement(price, pair.TickSize);
        }

        private static decimal RoundToIncrement(decimal val, decimal increment)
        {
            return Math.Round(val - val % increment + ((val % increment < increment / 2) ? 0.0m : increment), 8);
        }
    }
}