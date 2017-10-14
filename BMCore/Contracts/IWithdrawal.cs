using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCore.Models;

namespace BMCore.Contracts
{
    public interface IWithdrawal
    {
        string Uuid { get; set; }
        string Currency { get; set; }
        decimal Amount { get; set; }
        string Address { get; set; }
        decimal TxCost { get; set; }
        string TxId { get; set; }
        bool Pending { get; set; }
    }
}