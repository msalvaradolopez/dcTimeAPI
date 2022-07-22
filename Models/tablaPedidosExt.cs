using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class tablaPedidosDB
    {
        public string FOLIO { get; set; }
        public string SOCIO { get; set; }
        public string FECHA { get; set; }
        public string VENDEDOR { get; set; }
        public string ESTATUS { get; set; }
        
        public tablaPedidosDB() { }
        public tablaPedidosDB(string folio, string socio, string fecha, string vendedor, string estatus)
        {
            FOLIO = folio;
            SOCIO = socio;
            FECHA = fecha;
            VENDEDOR = vendedor;
            ESTATUS = estatus;
        }
    }
}