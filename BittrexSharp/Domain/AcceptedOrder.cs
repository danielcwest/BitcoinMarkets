using System;
using System.Collections.Generic;
using System.Text;
using BMCore.Models;

namespace BittrexSharp.Domain
{
    public class AcceptedOrder : IAcceptedAction
    {
        public string Uuid { get; set; }
    }
}
