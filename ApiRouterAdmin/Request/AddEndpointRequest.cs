namespace ApiRouterAdmin.Request
{
    /// <summary>
    /// Clase base
    /// </summary>
    /// <![CDATA[ 
    /// Autor: UNICOMER
    /// fecha creación: 19-07-022
    /// ]]>
    public class AddEndpointRequest
    {
        public Int64 p_aplicacion { get; set; }
        public string p_path { get; set; }
        public string p_descripcion { get; set; }
        public string p_jsonRequest { get; set; }
        public string p_jsonResponseErrorDefault { get; set; }
        public string p_metodoRestApi { get; set; }
        public Int32 p_estado { get; set; }

        public string auditoria { get; set; }
    }
}
