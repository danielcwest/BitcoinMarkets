using System;
using System.Collections.Generic;
using System.Text;
using BMCore.Contracts;

namespace BittrexSharp.Domain
{
    public class DepositAddress : IDepositAddress
    {
        public string Currency { get; set; }
        public string Address { get; set; }
    }
}