using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class BitacoraController : ApiController
    {

        [Route("api/Bitacora/Usuario/{user:int}")]
        [HttpGet]
        public Object GetBitacora(int user)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var bit = ProcessMsg.Bitacora.GetBitacoraByUsuario(user);
                return Content(HttpStatusCode.OK, bit);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Bitacora/Menu")]
        [HttpGet]
        public Object GetBitacora(string menu)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var bit = ProcessMsg.Bitacora.GetBitacoraByMenu(menu);
                return Content(HttpStatusCode.OK, bit);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }


    }
}
