using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class pedidosDB
    {
        public string Folio { get; set; }
        public string Socio { get; set; }
        public string Estatus { get; set; }
        public string Fecha { get; set; }
        public string SlpName { get; set; }
        public string FechaSurtiendo { get; set; }
        public string FechaCerrado { get; set; }
        public string FechaEntregado { get; set; }
        public string empID { get; set; }
        public string NomSurtidor { get; set; }
        public string Foto { get; set; }

        public pedidosDB() { }
        public pedidosDB(string folio, string socio, string estatus, string fecha, string slpname, string fechaSurtiendo, string fechaCerrado, string empid, string nomSurtidor, string foto, string pFechaEntregado) {
            Folio = folio;
            Socio = socio;
            Estatus = estatus;
            Fecha = fecha;
            SlpName = slpname;
            FechaSurtiendo = fechaSurtiendo == "" ? "" : fechaSurtiendo;
            FechaCerrado = fechaCerrado == "" ? "" : fechaCerrado;
            empID = empid;
            NomSurtidor = nomSurtidor;
            Foto = foto;
            FechaEntregado = pFechaEntregado;
    }
    }
}