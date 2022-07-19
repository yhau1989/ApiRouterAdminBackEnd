using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// Clase base
    /// </summary>
    /// <![CDATA[ 
    /// Autor: UNICOMER
    /// fecha creación: 19-07-022
    /// ]]>
    public class AllEndPoints
    {
        public Decimal id { get; set; }
        public Decimal aplicacion { get; set; }
        public string path { get; set; }
        public string descripcion { get; set; }
        public string jsonRequest { get; set; }
        public string jsonResponseErrorDefault { get; set; }
        public string metodoRestApi { get; set; }
        public Int64 estado { get; set; }
    }
}
