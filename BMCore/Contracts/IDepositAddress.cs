using System;
using System.Collections.Generic;
using System.Text;

namespace BMCore.Contracts
{
    public interface IDepositAddress
    {
        string Currency { get; set; }
        string Address { get; set; }
    }
}