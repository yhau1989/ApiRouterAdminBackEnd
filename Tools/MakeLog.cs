using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Microsoft.Extensions.Configuration;

namespace Tools
{
    /// <summary>
    /// Clase que permite garabar logs transaccionales en la base de datos.
    /// </summary>
    public class MakeLog
    {
        private int nivelLog;
        private Logger logger = null;

        /// <summary>
        /// Getter And Setter
        /// </summary>
        public int NivelLog
        {
            get
            {
                return nivelLog;
            }

            set
            {
                nivelLog = value;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerBase">Objeto base de la libreia NLog</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public MakeLog(Logger loggerBase)
        {
            try
            {
                logger = loggerBase;
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
                IConfiguration config = builder.Build();

                string nolog = config.GetValue<string>("Nlog:NoLog");
                string softlog = config.GetValue<string>("Nlog:SoftLog");
                string heavylog = config.GetValue<string>("Nlog:HeavyLog");
                string cConnection = config.GetValue<string>("ConnectionStrings:dbApiRouteOracle");
                string cConexion = testMD5.decrypt(cConnection);
                GlobalDiagnosticsContext.Set("connectionString", cConexion);

                if (nolog == "1")
                    nivelLog = 1;

                if (softlog == "1")
                    nivelLog = 2;

                if (heavylog == "1")
                    nivelLog = 3;
            }
            catch (Exception ex)
            {

                nivelLog = 0;
            }
        }

        /// <summary>
        /// Método para escribir logs de tipo informacion en la base de datos con Nlog
        /// </summary>
        /// <param name="mensaje">mensaje personalizado del error</param>
        /// <param name="app">aplicación/funcion desde donde se produjo el error</param>
        /// <param name="DataInput">Datos de enbreada que recibio la aplicación/funcion</param>
        /// <param name="DataOutput">Datos de salida que retorno la aplicación/funcion</param>
        /// <param name="TimeProces">Tiempo total que demoro la aplicación/funcion en ejecutarse</param>
        /// <param name="processRecord">Numero de registros que pudo haber procesado la aplicación/funcion</param>
        /// <param name="validarError">Bandera para identicar este log como error</param>
        /// <param name="client">Cliente o programa final que invoco la aplicación/funcion</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public void writeLog_info(string mensaje, string app, string? DataInput = null, string? DataOutput = null, string? TimeProces = null, string? processRecord = null, string? validarError = null, string? client = null)
        {

            if (nivelLog > 0)
            {
                //logger = LogManager.GetCurrentClassLogger();
                GlobalDiagnosticsContext.Set("App", app);
                if (DataInput != null) { GlobalDiagnosticsContext.Set("DataInput", DataInput); } else { GlobalDiagnosticsContext.Set("DataInput", ""); }
                if (DataOutput != null) { GlobalDiagnosticsContext.Set("DataOutput", DataOutput); } else { GlobalDiagnosticsContext.Set("DataOutput", ""); }
                if (TimeProces != null) { GlobalDiagnosticsContext.Set("TimeProces", TimeProces); } else { GlobalDiagnosticsContext.Set("TimeProces", ""); }
                if (processRecord != null) { GlobalDiagnosticsContext.Set("ProcessedRecords", processRecord); } else { GlobalDiagnosticsContext.Set("ProcessedRecords", ""); }
                if (client != null) { GlobalDiagnosticsContext.Set("client", client); } else { GlobalDiagnosticsContext.Set("client", ""); }
                if (validarError != null)
                {
                    GlobalDiagnosticsContext.Set("ValidarError", 1);
                    logger.Error(mensaje);
                }
                else
                {
                    GlobalDiagnosticsContext.Set("ValidarError", "");
                    logger.Info(mensaje);
                }

            }

        }


        /// <summary>
        /// Método para escribir logs de tipo trace en la base de datos con Nlog
        /// </summary>
        /// <param name="mensaje">mensaje personalizado del error</param>
        /// <param name="app">aplicación/funcion desde donde se produjo el error</param>
        /// <param name="DataInput">Datos de enbreada que recibio la aplicación/funcion</param>
        /// <param name="DataOutput">Datos de salida que retorno la aplicación/funcion</param>
        /// <param name="TimeProces">Tiempo total que demoro la aplicación/funcion en ejecutarse</param>
        /// <param name="processRecord">Numero de registros que pudo haber procesado la aplicación/funcion</param>
        /// <param name="validarError">Bandera para identicar este log como error</param>
        /// <param name="client">Cliente o programa final que invoco la aplicación/funcion</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public void writeLog_trace(string mensaje, string app, string? DataInput = null, string? DataOutput = null, string? TimeProces = null, string? processRecord = null, string? validarError = null, string? client = null)
        {

            if (nivelLog == 2 || nivelLog == 3)
            {
                //logger = LogManager.GetCurrentClassLogger();
                GlobalDiagnosticsContext.Set("App", app);
                if (DataInput != null) { GlobalDiagnosticsContext.Set("DataInput", DataInput); } else { GlobalDiagnosticsContext.Set("DataInput", ""); }
                if (DataOutput != null) { GlobalDiagnosticsContext.Set("DataOutput", DataOutput); } else { GlobalDiagnosticsContext.Set("DataOutput", ""); }
                if (TimeProces != null) { GlobalDiagnosticsContext.Set("TimeProces", TimeProces); } else { GlobalDiagnosticsContext.Set("TimeProces", ""); }
                if (processRecord != null) { GlobalDiagnosticsContext.Set("ProcessedRecords", processRecord); } else { GlobalDiagnosticsContext.Set("ProcessedRecords", ""); }
                if (client != null) { GlobalDiagnosticsContext.Set("client", client); } else { GlobalDiagnosticsContext.Set("client", ""); }
                if (validarError != null)
                {
                    GlobalDiagnosticsContext.Set("ValidarError", 1);
                    logger.Error(mensaje);
                }
                else
                {
                    GlobalDiagnosticsContext.Set("ValidarError", "");
                    logger.Trace(mensaje);
                }
            }
        }


        /// <summary>
        /// Método para escribir logs de tipo error en la base de datos con Nlog
        /// </summary>
        /// <param name="mensaje">mensaje personalizado del error</param>
        /// <param name="app">aplicación/funcion desde donde se produjo el error</param>
        /// <param name="DataInput">Datos de enbreada que recibio la aplicación/funcion</param>
        /// <param name="DataOutput">Datos de salida que retorno la aplicación/funcion</param>
        /// <param name="TimeProces">Tiempo total que demoro la aplicación/funcion en ejecutarse</param>
        /// <param name="processRecord">Numero de registros que pudo haber procesado la aplicación/funcion</param>
        /// <param name="validarError">Bandera para identicar este log como error</param>
        /// <param name="client">Cliente o programa final que invoco la aplicación/funcion</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay - UNICOMER
        /// fecha creación: 19-07-022
        /// ]]>
        public void writeLog_error(Exception error, string mensaje, string app, string? DataInput = null, string? DataOutput = null, string? TimeProces = null, string? processRecord = null, string? client = null)
        {
            //logger = LogManager.GetCurrentClassLogger();
            GlobalDiagnosticsContext.Set("App", app);
            if (DataInput != null) { GlobalDiagnosticsContext.Set("DataInput", DataInput); } else { GlobalDiagnosticsContext.Set("DataInput", ""); }
            if (DataOutput != null) { GlobalDiagnosticsContext.Set("DataOutput", DataOutput); } else { GlobalDiagnosticsContext.Set("DataOutput", ""); }
            if (TimeProces != null) { GlobalDiagnosticsContext.Set("TimeProces", TimeProces); } else { GlobalDiagnosticsContext.Set("TimeProces", ""); }
            if (processRecord != null) { GlobalDiagnosticsContext.Set("ProcessedRecords", processRecord); } else { GlobalDiagnosticsContext.Set("ProcessedRecords", ""); }
            if (client != null) { GlobalDiagnosticsContext.Set("client", client); } else { GlobalDiagnosticsContext.Set("client", ""); }
            GlobalDiagnosticsContext.Set("ValidarError", 1);
            logger.Error(error, mensaje);
        }
    }

}
