using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Contracts
{
    public interface IWithdrawal
    {
        string Uuid { get; set; }
        string Currency { get; set; }
        decimal Amount { get; set; }
        string Address { get; set; }
        string TxId { get; set; }
    }
}