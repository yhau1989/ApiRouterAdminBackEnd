namespace ApiRouterAdmin.Request
{
    public class UpdateAppRequest
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string dnsIpDestino { get; set; }
        public int estado { get; set; }

        public string auditoria { get; set; }
    }
}
