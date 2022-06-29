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
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getPedidos")]
        public IEnumerable<object> getPedidos([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.getPedidos();
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("setSurtiendo")]
        public string setSurtiendo([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.setSurtiendo(pFiltros);
            
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("setCerrado")]
        public string setCerrado([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.setCerrado(pFiltros);

        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getSurtidores")]
        public IEnumerable<object> getSurtidores([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.GetSurtidores();
        }
    }
}
