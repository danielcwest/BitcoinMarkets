using System;
using System.Collections.Generic;
using System.Text;
using Core.Contracts;
using Newtonsoft.Json;

namespace BittrexSharp.Domain
{
    public class BittrexWithdrawal
    {
        public string PaymentUuid { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Address { get; set; }
        public DateTime? Opened { get; set; }
        public bool? Authorized { get; set; }
        public bool? PendingPayment { get; set; }
        public decimal? TxCost { get; set; }
        public string TxId { get; set; }
        public bool? Canceled { get; set; }
        public bool? InvalidAddress { get; set; }
    }
    public class HistoricWithdrawal : IWithdrawal
    {
        public string Uuid { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public bool? Authorized { get; set; }
        public bool Pending { get; set; }
        public decimal TxCost { get; set; }
        public string TxId { get; set; }

        public HistoricWithdrawal(BittrexWithdrawal w)
        {
            Uuid = w.PaymentUuid;
            Currency = w.Currency;
            Amount = w.Amount.HasValue ? w.Amount.Value : 0m;
            Address = w.Address;
            Authorized = w.Authorized;
            Pending = w.PendingPayment.HasValue ? w.PendingPayment.Value : true;
            TxCost = w.TxCost.HasValue ? w.TxCost.Value : 0m;
            TxId = w.TxId;
        }
    }
}
