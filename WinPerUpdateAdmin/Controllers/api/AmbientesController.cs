using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiPrimerAPP.Controllers.api
{
    public class AmbientesController : ApiController
    {
        #region get
        [Route("api/Cliente/{idCliente:int}/Ambiente")]
        [HttpGet]
        public Object GetAmbientesByCliente(int idCliente)
        {
            try
            {
                var list = ProcessMsg.Ambiente.GetAmbientesByCliente(idCliente);
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
