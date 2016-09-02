using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProcessMsg;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class VersionController : ApiController
    {
        #region get
        // GET: api/Version
        [Route("api/Version")]
        [HttpGet]
        public Object Get()
        {
            try
            {
                var obj = ProcessMsg.Version.GetVersiones(null);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}")]
        [HttpGet]
        public Object Get(int idVersion)
        {
            try
            {
                var obj = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.VersionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion

        #region post
        [Route("api/Version")]
        [HttpPost]
        public Object Post([FromBody]ProcessMsg.Model.VersionBo version)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (version.Release == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Fecha == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Estado.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Version.AddVersion(version);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpPost]
        public Object PostComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null || archivo.Modulo == null || archivo.LastWrite == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Componente.AddComponente(idVersion, archivo);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion
    }
}
