﻿namespace ApiRouterAdmin.Request
{
    /// <summary>
    /// Clase base
    /// </summary>
    /// <![CDATA[ 
    /// Autor: UNICOMER
    /// fecha creación: 19-07-022
    /// ]]>
    public class AddAppRequest
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string dnsIpDestino { get; set; }
        public string auditoria { get; set; }

    }
}
