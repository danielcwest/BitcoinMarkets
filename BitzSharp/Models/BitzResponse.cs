using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitzSharp.Models
{
    public class BitzResponse<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }

    }
}