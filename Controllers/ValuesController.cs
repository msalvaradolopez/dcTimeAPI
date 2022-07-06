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

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getBanners")]
        public IEnumerable<object> getBanners([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.getBanners();
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insBanner")]
        public string insBanner([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.insBanner(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("setBanner")]
        public string setBanner([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.setBanner(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delBanner")]
        public string delBanner([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.delBanner(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getConPorDias")]
        public IEnumerable<object> getConPorDias([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.getConPorDias(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getConPorSurtidor")]
        public IEnumerable<object> getConPorSurtidor([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.getConPorSurtidor(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getConGeneral")]
        public IEnumerable<object> getConGeneral([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.getConGeneral(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("setFoto")]
        public string setFoto([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.setFoto(pFiltros);
        }
    }
}
