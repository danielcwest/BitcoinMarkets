using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public class Account
    {
        public decimal makerCommission { get; set; }
        public decimal takerCommission { get; set; }
        public decimal buyerCommission { get; set; }
        public decimal sellerCommission { get; set; }
        public bool canTrade { get; set; }
        public bool canWithdraw { get; set; }
        public bool canDeposit { get; set; }
        public List<BalanceResponse> balances { get; set; }
    }
}
