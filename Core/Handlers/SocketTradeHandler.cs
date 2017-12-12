using Core.Contracts;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{    
    public delegate Task SocketTradeHandler(IBaseSocketExchange sender, ArbitragePair pair, IMatch trade);
}
