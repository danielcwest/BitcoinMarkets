using RestEase;
using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp
{
    [Header("User-Agent", "NetCore API")]
    public interface ICoinbaseApi
    {
        [Header("CB-ACCESS-KEY")]
        string ApiKey { get; set; }

    }
}
