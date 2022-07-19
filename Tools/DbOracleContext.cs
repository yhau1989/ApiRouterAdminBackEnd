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

            //desencriptando cadena
            byte[] toBytes = Encoding.UTF8.GetBytes(cConexion);
            byte[] ketToBytes = Encoding.UTF8.GetBytes("xxxTokenxxx");
            cConexion = testMD5.decrypt(cConexion);
            //desencriptando cadena

            nameApp = config.GetValue<string>("nameApp");
            logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        }


        /// <summary>
        /// Valida el proceso del login
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg checkAuth(string user, string pass)
        {
            string funcion = "DbOracleContext.checkAuth";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;

            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"user: {user}, pass: {pass}");

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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);

            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;


        }


        /// <summary>
        /// obtiene todas las app del sistema desde la base de datos
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg getAllAppsData()
        {
            string funcion = "DbOracleContext.getAllAppsData";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp);

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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }


        /// <summary>
        /// obtiene todos los endpoints de las app del sistema desde la base de datos
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg getAllEnpoints()
        {
            string funcion = "DbOracleContext.getAllEnpoints";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp);

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_GetAllEndPoints";
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
                        data = new Conversores().ListEndPoints(dat)
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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, null, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }


        /// <summary>
        /// inserta aplicativo en la base de datos
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg insertApp(string p_nombre,
                                       string p_descripcion,
                                       string p_codigo,
                                       string p_dnsIpDestino)
        {
            string funcion = "DbOracleContext.insertApp";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"p_nombre: {p_nombre}, p_descripcion: {p_descripcion}, p_codigo: {p_codigo}, p_dnsIpDestino: {p_dnsIpDestino}");

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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, $"p_nombre: {p_nombre}, p_descripcion: {p_descripcion}, p_codigo: {p_codigo}, p_dnsIpDestino: {p_dnsIpDestino}", JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }




        /// <summary>
        /// actualiza aplicativo en la base de datos
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg updateApp(string p_nombre,
                                      string p_descripcion,
                                      string p_codigo,
                                      string p_dnsIpDestino,
                                      int p_estado)
        {
            string funcion = "DbOracleContext.updateApp";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"p_nombre: {p_nombre}, p_descripcion: {p_descripcion}, p_codigo: {p_codigo}, p_dnsIpDestino:{p_dnsIpDestino}, p_estado: {p_estado}");

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_UpdateApp";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_nombre", OracleDbType.Varchar2, p_nombre, ParameterDirection.Input);
                command.Parameters.Add("p_descripcion", OracleDbType.Varchar2, p_descripcion, ParameterDirection.Input);
                command.Parameters.Add("p_codigo", OracleDbType.Varchar2, p_codigo, ParameterDirection.Input);
                command.Parameters.Add("p_dnsIpDestino", OracleDbType.Varchar2, p_dnsIpDestino, ParameterDirection.Input);
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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, $"p_nombre: {p_nombre}, p_descripcion: {p_descripcion}, p_codigo: {p_codigo}, p_dnsIpDestino:{p_dnsIpDestino}, p_estado: {p_estado}", JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }




        /// <summary>
        /// inserta endpoints para los aplicativo en la base de datos
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg insertEnpoints(Int64 p_aplicacion,
                                            string p_path,
                                            string p_descripcion,
                                            string p_jsonRequest,
                                            string p_jsonResponseErrorDefault,
                                            string p_metodoRestApi,
                                            Int32 p_estado)
        {
            string funcion = "DbOracleContext.insertEnpoints";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            string fg = $"p_aplicacion: {p_aplicacion}, p_path:{p_path}, p_descripcion: {p_descripcion}, p_jsonRequest: {p_jsonRequest}, p_jsonResponseErrorDefault: {p_jsonResponseErrorDefault}, p_metodoRestApi: {p_metodoRestApi}, p_estado: {p_estado}";
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, fg);

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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, fg, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }



        /// <summary>
        /// actualiza los endpoints para los aplicativo en la base de datos
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg updateEnpoint(Int64 p_id,
                                            string p_path,
                                            string p_descripcion,
                                            string p_jsonRequest,
                                            string p_jsonResponseErrorDefault,
                                            string p_metodoRestApi,
                                            Int32 p_estado)
        {
            string funcion = "DbOracleContext.updateEnpoint";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            string df = $"p_id: {p_id}, p_path: {p_path}, p_descripcion: {p_descripcion}, p_jsonRequest: {p_jsonRequest}, p_jsonResponseErrorDefault: {p_jsonResponseErrorDefault}, p_metodoRestApi:{p_metodoRestApi}, p_estado: {p_estado}";
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, df);

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_UpdateEndPoint";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_id", OracleDbType.Int64, p_id, ParameterDirection.Input);
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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, df, JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }




        /// <summary>
        /// Valida aplicativos por el codigo del mismo
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg checkAppByCode(string codeApp)
        {
            string funcion = "DbOracleContext.checkAppByCode";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;

            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"codeApp: {codeApp}");

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_AppDataByCodeApp";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("codeApp", OracleDbType.Varchar2).Value = codeApp;
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
                        data = new Conversores().getApp(dat)
                    };
                }
                else
                {
                    rsp = new ResponseMsg()
                    {
                        status = 1,
                        msg = $"no se encontraron registros para el app: {codeApp}",
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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);

            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, $"codeApp: {codeApp}", JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;


        }



        /// <summary>
        /// Valida endpoints por el codigo del mismo
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg endpointsByIdApp(string idApp)
        {
            string funcion = "DbOracleContext.endpointsByIdApp";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;

            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"idApp: {idApp}");

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_EndPointsByIdApp";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("idApp", OracleDbType.Varchar2).Value = idApp;
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
                        data = new Conversores().ListEndPoints(dat)
                    };
                }
                else
                {
                    rsp = new ResponseMsg()
                    {
                        status = 1,
                        msg = $"no se encontraron registros para el app: {idApp}",
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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);

            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, $"idApp: {idApp}", JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;


        }



        /// <summary>
        /// elimina endpoints por el codigo del mismo.
        /// </summary>
        /// <![CDATA[ 
        /// Autor: UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public ResponseMsg deleteEndPoint(string id)
        {
            string funcion = "DbOracleContext.deleteEndPoint";
            DateTime time_inicio = DateTime.Now;
            ResponseMsg rsp;
            MakeLog log;
            try
            {

                log = new MakeLog(logger);
                log.writeLog_trace($"{funcion} , inició de llamada", nameApp, $"id: {id}");

                connection = new OracleConnection(cConexion);
                connection.Open();

                OracleCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PCK_API_ROUTER.proc_DeleteEndPoint";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("p_id", OracleDbType.Varchar2, id, ParameterDirection.Input);
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

                log = new MakeLog(logger);
                log.writeLog_error(ex, $"{funcion} , error", nameApp);
            }

            DateTime time_fin = DateTime.Now;
            TimeSpan ts = time_fin - time_inicio;
            log.writeLog_trace($"{funcion} , Fin de ejecución", nameApp, $"id: {id}", JsonConvert.SerializeObject(rsp), ts.ToString(@"hh\:mm\:ss\.fff"));

            return rsp;

        }


    }
}
