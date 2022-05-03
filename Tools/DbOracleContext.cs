using System;
using System.Collections.Generic;
using System.Data.Odbc;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using Newtonsoft.Json;
using NLog;

namespace Tools
{
    public class DbOracleContext
    {
        private string cConexion, nameApp = "";
        private string ConnectionString;
        IConfiguration config;
        private OracleConnection connection = null;
        private Logger logger = null;

        public DbOracleContext()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            config = builder.Build();
            cConexion = config.GetValue<string>("ConnectionStrings:dbApiRouteOracle");
            nameApp = config.GetValue<string>("nameApp");
            //logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        }

        public ResponseMsg checkAuth(string user, string pass)
        {
            //string funcion = "DbOracleContext.checkAuth";
            //DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            //MakeLog log;

            try
            {

                //log = new MakeLog(logger);
                //log.writeLog_trace($"{funcion} , inició de llamada", nameApp, JsonConvert.SerializeObject(user));

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_checkAuthLogin";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("p_username", OracleDbType.Varchar2).Value = user;
                command.Parameters.Add("p_password", OracleDbType.Varchar2).Value = pass;
                command.Parameters.Add("ci", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                DataTable usertab = new DataTable();

                OracleDataReader dataReader = command.ExecuteReader();
                usertab.Load(dataReader);

                command.Dispose();
                connection.Close();

                rsp = new ResponseMsg()
                {
                    status = (usertab.Rows.Count > 0) ? 0 : 1,
                    msg = (usertab.Rows.Count > 0) ? "ok" : $"usuario {user} no existe en la tabla tUsers, usuario o contraseña incorrectos",
                };
            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                rsp = new ResponseMsg()
                {
                    status = 99,
                    msg = ex.Message,
                };

                //log = new MakeLog(logger);
                //log.writeLog_error(ex, $"{funcion} , error", nameApp);

            }

            //DateTime time_fin = DateTime.Now;
            //TimeSpan ts = time_fin - time_inicio;
            //log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;


        }

        public ResponseMsg getAllAppsData()
        {
            //string funcion = "DbOracleContext.getAppData";
            //DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            //MakeLog log;
            try
            {

                //log = new MakeLog(logger);
                //log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"codeApp: {codeApp}", null, null, null, null, client, unicCode);

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_GetAllAppsActives";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("ci", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dat = new DataTable();

                OracleDataReader dataReader = command.ExecuteReader();
                dat.Load(dataReader);

                command.Dispose();
                connection.Close();

                if (dat.Rows.Count > 0)
                {
                    rsp = new ResponseMsg()
                    {
                        status = 0,
                        msg = "ok",
                        data = new Conversores().ListaApps(dat)
                    };
                }
                else
                {
                    rsp = new ResponseMsg()
                    {
                        status = 1,
                        msg = $"no se encontraron registros",
                    };
                }


            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                rsp = new ResponseMsg()
                {
                    status = 99,
                    msg = ex.Message,
                };

                //log = new MakeLog(logger);
                //log.writeLog_error(ex, $"{funcion} , error", nameApp, null, null, null, null, client, unicCode);
            }

            //DateTime time_fin = DateTime.Now;
            //TimeSpan ts = time_fin - time_inicio;
            //log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"), null, null, client, unicCode);

            return rsp;

        }

        public ResponseMsg insertApp(string p_nombre,
                                       string p_descripcion,
                                       string p_codigo,
                                       string p_dnsIpDestino)
        {
            //string funcion = "DbOracleContext.getAppData";
            //DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            //MakeLog log;
            try
            {

                //log = new MakeLog(logger);
                //log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"codeApp: {codeApp}", null, null, null, null, client, unicCode);

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_AddApp";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_nombre", OracleDbType.Varchar2, p_nombre, ParameterDirection.Input);
                command.Parameters.Add("p_descripcion", OracleDbType.Varchar2, p_descripcion, ParameterDirection.Input);
                command.Parameters.Add("p_codigo", OracleDbType.Varchar2, p_codigo, ParameterDirection.Input);
                command.Parameters.Add("p_dnsIpDestino", OracleDbType.Varchar2, p_dnsIpDestino, ParameterDirection.Input);

                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                rsp = new ResponseMsg()
                {
                    status = 0,
                    msg = "ok",
                };


            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                rsp = new ResponseMsg()
                {
                    status = 99,
                    msg = ex.Message,
                };

                //log = new MakeLog(logger);
                //log.writeLog_error(ex, $"{funcion} , error", nameApp, null, null, null, null, client, unicCode);
            }

            //DateTime time_fin = DateTime.Now;
            //TimeSpan ts = time_fin - time_inicio;
            //log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"), null, null, client, unicCode);

            return rsp;

        }



        public ResponseMsg insertEnpoints(Int64 p_aplicacion,
                                            string p_path,
                                            string p_descripcion,
                                            string p_jsonRequest,
                                            string p_jsonResponseErrorDefault,
                                            string p_metodoRestApi,
                                            Int32 p_estado)
        {
            //string funcion = "DbOracleContext.getAppData";
            //DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            //MakeLog log;
            try
            {

                //log = new MakeLog(logger);
                //log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"codeApp: {codeApp}", null, null, null, null, client, unicCode);

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_AddEndPoints";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_aplicacion", OracleDbType.Int64, p_aplicacion, ParameterDirection.Input);
                command.Parameters.Add("p_path", OracleDbType.Varchar2, p_path, ParameterDirection.Input);
                command.Parameters.Add("p_descripcion", OracleDbType.Varchar2, p_descripcion, ParameterDirection.Input);
                command.Parameters.Add("p_jsonRequest", OracleDbType.Varchar2, p_jsonRequest, ParameterDirection.Input);
                command.Parameters.Add("p_jsonResponseErrorDefault", OracleDbType.Varchar2, p_jsonResponseErrorDefault, ParameterDirection.Input);
                command.Parameters.Add("p_metodoRestApi", OracleDbType.Varchar2, p_metodoRestApi, ParameterDirection.Input);
                command.Parameters.Add("p_estado", OracleDbType.Int32, p_estado, ParameterDirection.Input);


                command.ExecuteNonQuery();

                command.Dispose();
                connection.Close();

                rsp = new ResponseMsg()
                {
                    status = 0,
                    msg = "ok",
                };


            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                rsp = new ResponseMsg()
                {
                    status = 99,
                    msg = ex.Message,
                };

                //log = new MakeLog(logger);
                //log.writeLog_error(ex, $"{funcion} , error", nameApp, null, null, null, null, client, unicCode);
            }

            //DateTime time_fin = DateTime.Now;
            //TimeSpan ts = time_fin - time_inicio;
            //log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"), null, null, client, unicCode);

            return rsp;

        }


    }
}
