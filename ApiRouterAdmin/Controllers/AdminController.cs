using ApiRouterAdmin.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Collections;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tools;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.HttpOverrides;

namespace ApiRouterAdmin.Controllers
{
    /// <summary>
    /// Clase para el manejo del controlador del administrador del endpoint /api
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay - UNICOMER
    /// fecha creación: 19-07-022
    /// ]]>
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        private IConfiguration config;
        private readonly IHttpClientFactory _httpClientFactory;
        private Logger logger = null;
        private string nameApp;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">objeto interzas</param>
        /// <param name="httpClientFactory">objeto interzas</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public AdminController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _config = configuration;
            _httpClientFactory = httpClientFactory;

            logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            nameApp = config.GetValue<string>("nameApp");
        }


        /// <summary>
        /// Realiza el proceso de login contra la base de datos
        /// </summary>
        /// <param name="request">Clase base LoginRequest</param>
        /// <returns>ResponseMsg clase base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/login"), HttpPost]
        public async Task<ResponseMsg> login(LoginRequest request)
        {
            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.login, inició de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", null, null, null, null, "");


            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.checkAuth(request.user, request.password);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.login, fin de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, "");


            return result;
        }


        /// <summary>
        /// Retorna una lista de la base de datos
        /// </summary>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/allappactives"), HttpGet]
        public async Task<ResponseMsg> getAllApps()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.getAllApps, inició de llamada", nameApp , "", null, null, null, null, remoteIpAddress);

            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.getAllAppsData();


            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.getAllApps, fin de llamada", nameApp, "", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, remoteIpAddress);

            return result;
        }


        /// <summary>
        /// Retorna una lista con todos los endpoint desde la base de datos
        /// </summary>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/allaendpoints"), HttpGet]
        public async Task<ResponseMsg> getAllEndPoints()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.getAllEndPoints, inició de llamada", nameApp, "", null, null, null, null, remoteIpAddress);

            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.getAllEnpoints();

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.getAllEndPoints, fin de llamada", nameApp, "", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, remoteIpAddress);

            return result;
        }


        /// <summary>
        /// Agrega una nueva app a la base de datos
        /// </summary>
        /// <param name="request">Clase base AddAppRequest</param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/addapp"), HttpPost]
        public async Task<ResponseMsg> addApp(AddAppRequest request)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.addApp, inició de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", null, null, null, null, request.auditoria + $" , {remoteIpAddress}");

            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.insertApp(request.nombre, request.descripcion, request.codigo, request.dnsIpDestino);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.addApp, fin de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, request.auditoria + $" , {remoteIpAddress}");

            return result;
        }


        /// <summary>
        /// Actualiza la informacion de una app existente en la base de datos.
        /// </summary>
        /// <param name="request">Clase base UpdateAppRequest</param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/updateapp"), HttpPost]
        public async Task<ResponseMsg> updateApp(UpdateAppRequest request)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.updateApp, inició de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", null, null, null, null, request.auditoria + $" , {remoteIpAddress}");


            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.updateApp(request.nombre, request.descripcion, request.codigo, request.dnsIpDestino, request.estado);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio; 
            log.writeLog_trace($"AdminController.updateApp, fin de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, request.auditoria + $" , {remoteIpAddress}");

            return result;
        }


        /// <summary>
        /// Agrega un nuevo endpoint a una app existente en la base de datos.
        /// </summary>
        /// <param name="request">Clase base AddEndpointRequest</param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/addendpoint"), HttpPost]
        public async Task<ResponseMsg> addEndpoint(AddEndpointRequest request)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.addEndpoint, inició de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", null, null, null, null, request.auditoria + $" , {remoteIpAddress}");


            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.insertEnpoints(request.p_aplicacion,
                                                   request.p_path,
                                                   request.p_descripcion,
                                                   request.p_jsonRequest,
                                                   request.p_jsonResponseErrorDefault,
                                                   request.p_metodoRestApi,
                                                   request.p_estado);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.addEndpoint, fin de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, request.auditoria + $" , {remoteIpAddress}");

            return result;
        }


        /// <summary>
        /// Actualiza un endpoint de una app existente en la base de datos.
        /// </summary>
        /// <param name="request">Clase base UpdateEndpointRequest</param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/updateepoint"), HttpPost]
        public async Task<ResponseMsg> updateEndpoint(UpdateEndpointRequest request)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.updateEndpoint, inició de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", null, null, null, null, request.auditoria + $" , {remoteIpAddress}");


            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.updateEnpoint(request.p_id,
                                                   request.p_path,
                                                   request.p_descripcion,
                                                   request.p_jsonRequest,
                                                   request.p_jsonResponseErrorDefault,
                                                   request.p_metodoRestApi,
                                                   request.p_estado);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.updateEndpoint, fin de llamada", nameApp, $"request: {JsonConvert.SerializeObject(request)}", $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, request.auditoria + $" , {remoteIpAddress}");


            return result;
        }

        /// <summary>
        /// Retorna todos los datos de una app segun su codigo unico
        /// </summary>
        /// <param name="codeApp">Cosdigo de la app</param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/appbycode"), HttpGet]
        public async Task<ResponseMsg> getAppByCode(string codeApp)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.getAppByCode, inició de llamada", nameApp, codeApp, null, null, null, null, remoteIpAddress);


            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.checkAppByCode(codeApp);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.getAppByCode, fin de llamada", nameApp, codeApp, $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, remoteIpAddress);

            return result;
        }


        /// <summary>
        /// Retorna una lista de endpoints de una app segun su id de App
        /// </summary>
        /// <param name="idApp"></param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/endpointsbyidap"), HttpGet]
        public async Task<ResponseMsg> endpointsByIdApp(string idApp)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.endpointsByIdApp, inició de llamada", nameApp, idApp, null, null, null, null, remoteIpAddress);

            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.endpointsByIdApp(idApp);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.endpointsByIdApp, fin de llamada", nameApp, idApp, $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, remoteIpAddress);

            return result;
        }


        /// <summary>
        /// Elimina un endpoint existente en la base de datos 
        /// </summary>
        /// <param name="id">id del Endpoint</param>
        /// <param name="auditoria"></param>
        /// <returns>ResponseMsg Objeto Base</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        [Route("api/removeendpoint"), HttpPost]
        public async Task<ResponseMsg> deleteEndpointsById(string id, string auditoria)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            DateTime time_inicio = DateTime.Now;
            MakeLog log = new MakeLog(logger);
            log.writeLog_trace($"AdminController.deleteEndpointsById, inició de llamada", nameApp, id, null, null, null, null, auditoria + $" , {remoteIpAddress}");

            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.deleteEndPoint(id);

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"AdminController.deleteEndpointsById, fin de llamada", nameApp, id, $"response: {JsonConvert.SerializeObject(result)}", ts.ToString(@"hh\:mm\:ss\.fff"), null, null, auditoria + $" , {remoteIpAddress}");

            return result;
        }

    }
}
