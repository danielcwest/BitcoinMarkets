using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Models;

namespace BMCore
{
    public class Helper
    {

        public static List<OrderBookEntry> SumOrderEntries(List<OrderBookEntry> entries)
        {
            for (var i = 0; i < entries.Count; i++)
            {
                if (i == 0)
                {
                    entries[0].sum = entries[0].quantity;
                    entries[0].sumBase = entries[0].quantity * entries[0].price;
                }
                else
                {
                    entries[i].sum = entries[i - 1].sum + entries[i].quantity;
                    entries[i].sumBase = entries[i - 1].sumBase + (entries[i].quantity * entries[i].price);
                }
            }

            return entries;
        }
    }
}