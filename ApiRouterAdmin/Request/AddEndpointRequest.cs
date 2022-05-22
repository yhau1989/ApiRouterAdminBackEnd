namespace ApiRouterAdmin.Request
{
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
