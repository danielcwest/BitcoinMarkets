using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public class IAcceptedResponse : IAcceptedAction
    {
        public string Uuid { get; set; }
    }
}
