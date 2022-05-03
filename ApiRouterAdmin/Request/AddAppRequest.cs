namespace ApiRouterAdmin.Request
{
    public class AddAppRequest
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string dnsIpDestino { get; set; }
    }
}
