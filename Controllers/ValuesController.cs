using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Threading.Tasks;
using System.IO;

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

        [AcceptVerbs("GET")]
        [HttpGet()]
        [Route("getPublicidad")]
        public HttpResponseMessage getPublicidad()
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/") + "Publicidad.MP4";

            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.  
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", "Publicidad.MP4");
                throw new HttpResponseException(response);
            }
            //Read the File into a Byte Array.  
            byte[] bytes = File.ReadAllBytes(filePath);
            //Set the Response Content.  
            response.Content = new ByteArrayContent(bytes);
            //Set the Response Content Length.  
            response.Content.Headers.ContentLength = bytes.LongLength;
            //Set the Content Disposition Header Value and FileName.  
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "Publicidad.MP4";
            //Set the File Content Type.  
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping("Publicidad.MP4"));
            return response;
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("setUpLoadFile")]
        public string setUpLoadFile([FromBody] IFiltros pFiltros)
        {
            conexiones lConexiones = new conexiones();
            return lConexiones.setParam(pFiltros);
        }

        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("setPublicidad")]
        public async Task<string> setPublicidad()
        {
            var ctx = HttpContext.Current;
            var lroot = ctx.Server.MapPath("~/App_Data");
            var lprovider = new MultipartFormDataStreamProvider(lroot);

            try
            {
                await Request.Content.ReadAsMultipartAsync(lprovider);

                foreach (var file in lprovider.FileData)
                {
                    // var name = file.Headers.ContentDisposition.FileName;
                    var name = "Publicidad.MP4";
                    name = name.Trim('"');

                    
                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(lroot, name);
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    File.Move(localFileName, filePath);
                }
            }
            catch (Exception e)
            {

                return e.Message;
            }

            conexiones lConexiones = new conexiones();
            return "Publicidad cargada.";
            // return lConexiones.setParam(pVideo);
        }
    }
}
