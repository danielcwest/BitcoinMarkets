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
            this.Currency = string.IsNullOrWhiteSpace(tx.bitcoin_address) ? tx.currency_code_from : "BTC";
            this.Address = tx.destination_data.ToString();
            this.Amount = Convert.ToDecimal(tx.amount_from);
            this.TxId = tx.external_data;
            this.Pending = tx.status == "pending";

        }
    }
}