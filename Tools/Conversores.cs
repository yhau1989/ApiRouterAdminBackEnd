using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// Clase para convertir dataTable de Oracle a DataTables de c#
    /// </summary>
    /// <![CDATA[ 
    /// Autor: UNICOMER
    /// fecha creación: 19-07-022
    /// ]]>
    public class Conversores
    {

        public List<ListsApps> ListaApps(DataTable tab)
        {
            List<ListsApps> lista = new List<ListsApps>();

            foreach (DataRow row in tab.Rows)
            {
                lista.Add(new ListsApps()
                {
                    id = row.Field<Int64>("id"),
                    nombre = row.Field<string>("nombre"),
                    descripcion = row.Field<string>("descripcion"),
                    codigo = row.Field<string>("codigo"),
                    dnsIpDestino = row.Field<string>("dnsIpDestino"),
                    estado = row.Field<Int64>("estado"),
                });
            }

            return lista;

        }


        public List<AllEndPoints> ListEndPoints(DataTable tab)
        {
            List<AllEndPoints> lista = new List<AllEndPoints>();

            foreach (DataRow row in tab.Rows)
            {
                lista.Add(new AllEndPoints()
                {
                    id = row.Field<Decimal>("id"),
                    aplicacion = row.Field<Decimal>("aplicacion"),
                    path = row.Field<string>("path"),
                    descripcion = row.Field<string>("descripcion"),
                    jsonRequest = row.Field<string>("jsonRequest"),
                    jsonResponseErrorDefault = row.Field<string>("jsonResponseErrorDefault"),
                    metodoRestApi = row.Field<string>("metodoRestApi"),
                    estado = row.Field<Int64>("estado"),
                });
            }

            return lista;

        }

        public App getApp(DataTable tab)
        {
            DataRow row = tab.Rows[0];
            return new App()
            {
                id = row.Field<Int64>("id"),
                nombre = row.Field<string>("nombre"),
                descripcion = row.Field<string>("descripcion"),
                codigo = row.Field<string>("codigo"),
                dnsIpDestino = row.Field<string>("dnsIpDestino"),
                estado = row.Field<Int64>("estado"),
            };

        }

    }
}
