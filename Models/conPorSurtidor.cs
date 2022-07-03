using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class conPorSurtidor
    {
        public string surtidor { get; set; }
        public string porSurtir { get; set; }
        public string surtiendo { get; set; }
        public string empID { get; set; }

        public conPorSurtidor() { }

        public conPorSurtidor(string pSurtidor, string pPorsurtir, string pSurTiendo, string pEmpID)
        {
            surtidor = pSurtidor;
            porSurtir = pPorsurtir;
            surtiendo = pSurTiendo;
            empID = pEmpID;
        }
    }
}