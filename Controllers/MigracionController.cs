using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dcTimeAPI.Models;
using dcTimeAPI.dataBase;


namespace dcTimeAPI.Controllers
{
    [RoutePrefix("api/migracion")]
    public class MigracionController : ApiController
    {
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("migSurtidores")]
        public string migSurtidores()
        {
            migraConsultas lConexiones = new migraConsultas();
            lConexiones.migSurtidores();
            return "OK";

        }

    }
}
