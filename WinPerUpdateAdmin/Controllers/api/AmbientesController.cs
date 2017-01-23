using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MiPrimerAPP.Controllers.api
{
    public class AmbientesController : ApiController
    {
        #region get
        [Route("api/AmbientesXLSX/Planilla")]
        [HttpGet]
        public Object GetPlanillaAmbientesXLSX()
        {
            try
            {
                string dirfmt = string.Format("{0}", ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes/")) + "PlanillaAmbientes.xlsx");
                
                Byte[] objByte = System.IO.File.ReadAllBytes(dirfmt);
                if (objByte == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                }
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);

                message.Content = new ByteArrayContent(objByte);
                message.Content.Headers.ContentLength = objByte.Length;
                message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                message.Content.Headers.Add("Content-Disposition", "attachment; filename=PlanillaAmbientes.xlsx");
                message.StatusCode = HttpStatusCode.OK;

                return message;

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/Cliente/{idCliente:int}/AmbientesXLSX")]
        [HttpGet]
        public Object GetAmbientesXLSX(int idCliente)
        {
            try
            {
                return ProcessMsg.Ambiente.GetAmbientesXLSX(idCliente).OrderBy(x => x.idAmbientes);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


        [Route("api/Cliente/{idCliente:int}/Version/{idVersion:int}/{NameFile}/Ambientes")]
        [HttpGet]
        public Object GetAmbientesNoEx(int idCliente, int idVersion, string NameFile)
        {
            try
            {
                return ProcessMsg.Ambiente.GetAmbientesNoEx(idCliente, idVersion, NameFile).Where(x => !x.EjecutadoOK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Ambiente/{idAmbiente:int}/Version/{idVersion:int}/OK")]
        [HttpGet]
        public Object AmbienteOK(int idAmbiente, int idVersion)
        {
            try
            {
                return ProcessMsg.Ambiente.AmbienteOK(idVersion, idAmbiente);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Cliente/{idCliente:int}/Ambiente")]
        [HttpGet]
        public Object GetAmbientesByCliente(int idCliente)
        {
            try
            {
                var list = ProcessMsg.Ambiente.GetAmbientesByCliente(idCliente, null);
                if (list.Count == 0)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.AmbienteBo)null);
                }
                return list.OrderBy(x => x.Nombre);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Cliente/{idCliente:int}/Version/{idVersion:int}/Ambiente")]
        [HttpGet]
        public Object GetAmbientesByVersion(int idCliente, int idVersion)
        {
            try
            {
                var list = ProcessMsg.Ambiente.GetAmbientesByCliente(idCliente, idVersion, null);
                if (list.Count == 0)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.AmbienteBo)null);
                }
                return list.OrderBy(x => x.Nombre);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Cliente/{idCliente:int}/Ambiente/{idAmbiente:int}")]
        [HttpGet]
        public Object Get(int idCliente,int idAmbiente)
        {
            try
            {
                var obj = ProcessMsg.Ambiente.GetAmbiente(idAmbiente, idCliente);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.AmbienteBo)null);
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
        [Route("api/Cliente/{idCliente:int}/Ambiente")]
        [HttpPost]
        public Object Post(int idCliente, [FromBody]ProcessMsg.Model.AmbienteBo ambiente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
                var obj = ProcessMsg.Ambiente.Add(idCliente,ambiente);
                if (obj == null)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
                }
                else
                {
                    return Content(HttpStatusCode.Created, obj);
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
        [Route("api/Cliente/{idCliente:int}/Ambiente/{idAmbiente:int}")]
        [HttpPut]
        public Object Put(int idCliente, int idAmbiente, [FromBody]ProcessMsg.Model.AmbienteBo ambiente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                var obj = ProcessMsg.Ambiente.Update(idCliente, idAmbiente, ambiente);
                if (obj == null)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
                }
                else
                {
                    return Content(HttpStatusCode.Created, obj);
                }
                return Content(response.StatusCode, (ProcessMsg.Model.AmbienteBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion

        #region delete
        [Route("api/Cliente/{idCliente:int}/Ambiente/{idAmbiente:int}")]
        [HttpDelete]
        public Object Delete(int idCliente, int idAmbiente)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
                if (ProcessMsg.Ambiente.Delete(idAmbiente,idCliente) <= 0)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
                }
                return Content(response.StatusCode, (ProcessMsg.Model.ClienteBo)null);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion
    }
}
