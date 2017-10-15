using System;
using BMCore.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace HitbtcSharp.Models
{
    public class Withdrawal : IWithdrawal
    {
        public string Uuid { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public decimal TxCost { get; set; }
        public string TxId { get; set; }
        public bool Pending { get; set; }

        public Withdrawal(Transaction tx)
        {
            this.Uuid = tx.id;
            this.Currency = tx.currency;
            this.Address = tx.address;
            this.Amount = tx.amount;
            this.TxId = tx.hash;
            this.Pending = tx.status == "pending";
            this.TxCost = tx.networkFee + tx.fee;
        }
    }
}