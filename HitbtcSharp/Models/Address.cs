using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitbtcSharp.Models
{
    public class Address
    {
        /// <summary>
        /// BTC/LTC address to withdraw to
        /// </summary>
        public string address { get; set; }
        public string paymentId { get; set; }
    }
}
