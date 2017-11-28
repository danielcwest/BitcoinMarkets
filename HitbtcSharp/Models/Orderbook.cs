using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitbtcSharp.Models
{
    public class BookEntry
    {
        public decimal price { get; set; }
        public decimal size { get; set; }
    }
    public class OrderbookResponse
    {
        public List<BookEntry> ask { get; set; }
        public List<BookEntry> bid { get; set; }
    }

    public class RpcOrderbookResponse
    {
        public List<BookEntry> ask { get; set; }
        public List<BookEntry> bid { get; set; }
        public string symbol { get; set; }
        public long sequence { get; set; }
    }
}
