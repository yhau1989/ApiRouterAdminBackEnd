using ApiRouterAdmin.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using NLog;
using System.Collections;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tools;
using static System.Net.Mime.MediaTypeNames;

namespace ApiRouterAdmin.Controllers
{
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;
        private IConfiguration config;
        private readonly IHttpClientFactory _httpClientFactory;
        private string nameApp;

        public AdminController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _config = configuration;
            _httpClientFactory = httpClientFactory;

            // logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            nameApp = config.GetValue<string>("nameApp");
        }

        [Route("api/login"), HttpPost]
        public async Task<ResponseMsg> login(LoginRequest request)
        {
            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.checkAuth(request.user, request.password);

            return result;
        }

        [Route("api/allappactives"), HttpGet]
        public async Task<ResponseMsg> getAllApps()
        {
            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.getAllAppsData();

            return result;
        }

        [Route("api/addapp"), HttpPost]
        public async Task<ResponseMsg> addApp(AddAppRequest request)
        {
            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.insertApp(request.nombre, request.descripcion, request.codigo, request.dnsIpDestino);

            return result;
        }


        [Route("api/addendpoint"), HttpPost]
        public async Task<ResponseMsg> addEndpoint(AddEndpointRequest request)
        {
            DbOracleContext bd = new DbOracleContext();
            ResponseMsg result = bd.insertEnpoints(request.p_aplicacion,
                                                   request.p_path,
                                                   request.p_descripcion,
                                                   request.p_jsonRequest,
                                                   request.p_jsonResponseErrorDefault,
                                                   request.p_metodoRestApi,
                                                   request.p_estado);

            return result;
        }

    }
}
