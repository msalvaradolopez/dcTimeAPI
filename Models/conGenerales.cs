using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class conGenerales
    {
        public string folio { get; set; }
        public string statusText { get; set; }
        public string fecha { get; set; }
        public string porSurtir { get; set; }
        public string fechaSurtiendo { get; set; }
        public string surtiendo { get; set; }
        public string fechaCerrado { get; set; }
        public string firstName { get; set; }
        public string socio { get; set; }
        public string SlpName { get; set; }
        public string empID { get; set; }

        public conGenerales() { }

        public conGenerales(string pFolio, string pStatusText, string pFecha, string pPorSurtir, string pFechaSurtiendo, string pSurtiendo, string pFechaCerrado, string pFirstName, string pSocio, string pSlpName, string pEmpID) {
            folio = pFolio;
            statusText = pStatusText;
            fecha = pFecha;
            porSurtir = pPorSurtir;
            fechaSurtiendo = pFechaSurtiendo;
            surtiendo = pSurtiendo;
            fechaCerrado = pFechaCerrado;
            firstName = pFirstName;
            socio = pSocio;
            SlpName = pSlpName;
            empID = pEmpID;
        }

    }
}