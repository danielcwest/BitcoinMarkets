using System;
using System.Collections.Generic;
using System.Text;
using Core.Contracts;

namespace Core.Models
{
    public class DepositAddress : IDepositAddress
    {
        public string Currency { get; set; }
        public string Address { get; set; }
        public string Tag { get; set; }
    }
}