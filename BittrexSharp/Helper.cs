using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BittrexSharp
{
    public class Helper
    {
        public static string GetSourceCurrencyFromMarketName(string marketName) => marketName.Split('-').First();
        public static string GetTargetCurrencyFromMarketName(string marketName) => marketName.Split('-').Last();

        //return Bittrex market name in the form of BTC-XMR or ETH-XRP
        public static string GetMarketNameFromSymbol(string symbol)
        {
            if (symbol.EndsWith("BTC"))
            {
                return string.Format("BTC-{0}", symbol.Replace("BTC", ""));
            }
            else if (symbol.EndsWith("ETH"))
            {
                return string.Format("ETH-{0}", symbol.Replace("ETH", ""));
            }
            else
            {
                return null;
            }
        }

        public static string GetSymbolFromMarketName(string marketName) =>
            string.Format("{0}{1}", GetTargetCurrencyFromMarketName(marketName), GetSourceCurrencyFromMarketName(marketName));
    }
}
