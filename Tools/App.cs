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
    public class App
    {
        public Int64 id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string dnsIpDestino { get; set; }
        public Int64 estado { get; set; }

    }
}
