using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class conPorDias
    {
        public string fechaOrigen { get; set; }
        public string porSurtir { get; set; }
        public string surtiendo { get; set; }

        public conPorDias() { }

        public conPorDias(string fechaorigen, string porsurtir, string surTiendo) {
            fechaOrigen = fechaorigen;
            porSurtir = porsurtir;
            surtiendo = surTiendo;
        }
    }
}