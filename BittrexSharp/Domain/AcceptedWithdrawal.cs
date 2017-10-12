using System;
using System.Collections.Generic;
using System.Text;
using BMCore.Models;

namespace BittrexSharp.Domain
{
    public class AcceptedWithdrawal : IAcceptedAction
    {
        public string Uuid { get; set; }
    }
}
