﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.Contracts;

namespace BittrexSharp.Domain
{
    public class AcceptedOrder : IAcceptedAction
    {
        public string Uuid { get; set; }
    }
}
