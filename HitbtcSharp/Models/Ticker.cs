using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitbtcSharp.Models
{
    public class Ticker
    {
        /// <summary>
        /// Last price
        /// </summary>
        public decimal? last { get; set; }

        /// <summary>
        /// Highest buy order
        /// </summary>
        public decimal? bid { get; set; }

        /// <summary> 
        /// Lowest sell order
        /// </summary>
        public decimal? ask { get; set; }

        /// <summary>
        /// Highest trade price per last 24h + last incomplete minute
        /// </summary>
        public decimal? high { get; set; }

        /// <summary>
        /// Lowest trade price per last 24h + last incomplete minute
        /// </summary>
        public decimal? low { get; set; }

        /// <summary>
        /// Volume per last 24h + last incomplete minute
        /// </summary>
        public decimal? volume { get; set; }

        /// <summary>
        /// Price in which instrument open
        /// </summary>
        public decimal? open { get; set; }

        /// <summary>
        /// Volume in second currency per last 24h + last incomplete minute
        /// </summary>
        public decimal? volume_quote { get; set; }

        /// <summary>
        /// Server time in UNIX timestamp format
        /// </summary>
        public long timestamp { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"last:{last}");
            sb.AppendLine($"bid:{bid}");
            sb.AppendLine($"ask:{ask}");
            sb.AppendLine($"high:{high}");
            sb.AppendLine($"low:{low}");
            sb.AppendLine($"volume:{volume}");
            sb.AppendLine($"open:{open}");
            sb.AppendLine($"volume_quote:{volume_quote}");
            sb.AppendLine($"timestamp:{timestamp}");

            return sb.ToString();
        }
    }
}
