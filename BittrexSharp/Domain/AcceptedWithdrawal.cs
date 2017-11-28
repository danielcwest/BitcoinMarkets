using System;
using System.Collections.Generic;
using System.Text;
using Core.Contracts;

namespace BittrexSharp.Domain
{
    public class AcceptedWithdrawal : IAcceptedAction
    {
        public string Uuid { get; set; }
    }
}
