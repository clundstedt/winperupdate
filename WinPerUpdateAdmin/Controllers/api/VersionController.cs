using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


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

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpGet]
        public Object GetComponentes(int idVersion)
        {
            try
            {
                var obj = ProcessMsg.Componente.GetComponentes(idVersion, null);
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

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpGet]
        public Object GetComponentes(int idVersion, string nameFile)
        {
            try
            {
                var obj = ProcessMsg.Componente.GetComponentes(idVersion, null).SingleOrDefault(x => x.Name.Equals(nameFile));
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

        [Route("api/Version/{idVersion:int}/Cliente/{idCliente:int}")]
        [HttpPost]
        public Object PostVersionCliente(int idVersion, int idCliente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (idVersion <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (idCliente <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var retorno = ProcessMsg.Version.AddVersionCliente(idVersion, idCliente);
                    if (retorno == 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, (ProcessMsg.Model.VersionBo)null);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Cliente/{idCliente:int}/Ambiente")]
        [HttpPost]
        public Object PostVersionAmbienteCliente(int idVersion, int idCliente, [FromBody]ProcessMsg.Model.AmbienteBo ambiente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (idVersion <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (idCliente <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var retorno = ProcessMsg.Version.AddVersionAmbiente(idVersion, ambiente.idAmbientes, idCliente, ambiente.Estado);
                    if (retorno == 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.Created, (ProcessMsg.Model.AmbienteBo)null);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AmbienteBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion

        #region put
        [Route("api/Version/{idVersion:int}")]
        [HttpPut]
        public Object Put(int idVersion, [FromBody]ProcessMsg.Model.VersionBo version)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (version.Release == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (version.Fecha == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Version.UpdVersion(idVersion, version);
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
        [HttpPut]
        public Object PutComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null || archivo.Modulo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var obj = ProcessMsg.Componente.UpdComponente(idVersion, archivo);
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

        #region delete
        [Route("api/Version/{idVersion:int}")]
        [HttpDelete]
        public Object DeleteVersion(int idVersion)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (ProcessMsg.Version.DelVersion(idVersion) <= 0)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
                }

                return Content(response.StatusCode, (ProcessMsg.Model.AtributosArchivoBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Version/{idVersion:int}/Componentes")]
        [HttpDelete]
        public Object DeleteComponentes(int idVersion, [FromBody]ProcessMsg.Model.AtributosArchivoBo archivo)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (archivo == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (archivo.Name == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (ProcessMsg.Componente.DelComponente(idVersion, archivo) <= 0)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
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
