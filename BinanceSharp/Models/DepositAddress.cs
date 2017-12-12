using System;
using System.Collections.Generic;
using System.Text;
using Core.Contracts;

namespace BinanceSharp.Models
{
    public class DepositAddress : IDepositAddress
    {
        public string Currency { get; set; }
        public string Address { get; set; }
        public string Tag { get; set; }

        public DepositAddress(AddressResponse addr)
        {
            Currency = addr.asset;
            Address = addr.address;
            Tag = addr.addressTag;
        }
    }

    public class AddressResponse
    {
        public string address { get; set; }
        public bool success { get; set; }
        public string addressTag { get; set; }
        public string asset { get; set; }
    }
}