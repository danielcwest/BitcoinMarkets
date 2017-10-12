using System;
using System.Collections.Generic;
using System.Text;

namespace BMCore.Models
{
    public class DepositAddress : IDepositAddress
    {
        public string Currency { get; set; }
        public string Address { get; set; }
    }
}