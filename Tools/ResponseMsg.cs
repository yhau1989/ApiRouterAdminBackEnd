using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class ResponseMsg
    {
        public int status { get; set; }
        public string? msg { get; set; }
        public Object? data { get; set; }
        //public string? uniTransac { get; set; } = null;
    }
}
