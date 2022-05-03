using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Conversores
    {

        public List<ListsApps>  ListaApps(DataTable tab)
        {
            List<ListsApps> lista = new List<ListsApps>();

            foreach(DataRow row in tab.Rows)
            {
                lista.Add(new ListsApps()
                {
                    id = row.Field<Int64>("id"),
                    nombre = row.Field<string>("nombre"),
                    descripcion = row.Field<string>("descripcion"),
                    codigo = row.Field<string>("codigo"),
                    dnsIpDestino = row.Field<string>("dnsIpDestino")
                });
            }

            return lista;

        }




    }
}
