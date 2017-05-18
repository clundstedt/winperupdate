using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class DescargasController : ApiController
    {
        #region Classes
        public class Descarga
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string NombreArchivo { get; set; }
        }
        #endregion

        #region Gets
        [Route("api/Descargas")]
        [HttpGet]
        public Object GetDescargas()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                List<Descarga> ListaDescargas = new List<Descarga>();
                var pathFuentes = ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes"));
                var pathDescargas = Path.Combine(pathFuentes, "Descargas");
                new DirectoryInfo(pathDescargas).GetDirectories().ToList().OrderBy(dir => dir.Name).ToList().ForEach(dirs =>
                {
                    Descarga descarga = new Descarga();
                    descarga.Nombre = dirs.Name;
                    var desc = dirs.GetFiles().ToList().SingleOrDefault(fil => (fil.Name.Equals("Descripcion.txt", StringComparison.OrdinalIgnoreCase)));//obtiene el archivo txt con la descripcion de la descarga
                    descarga.Descripcion = File.ReadAllText(desc.FullName);
                    var file = dirs.GetFiles().ToList().SingleOrDefault(fil => ((fil.Attributes & FileAttributes.System) != FileAttributes.System) && !(fil.Extension.Equals(".txt", StringComparison.OrdinalIgnoreCase)));//obtiene el archivo comprimido que se descargara
                    descarga.NombreArchivo = file.Name;
                    ListaDescargas.Add(descarga);
                });
                return Content(HttpStatusCode.OK, ListaDescargas);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Descargar/{Archivo}/Directorio/{Dir}/Down")]
        [HttpGet]
        public Object GetDescargaArchivo(string Archivo, string Dir)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var pathArchivo = Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes")),"Descargas", Dir, Archivo);
                if (File.Exists(pathArchivo))
                {
                    var fileInfo = new FileInfo(pathArchivo);

                    Byte[] objByte = System.IO.File.ReadAllBytes(fileInfo.FullName);
                    if (objByte == null)
                    {
                        return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                    }
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);
                    
                    message.Content = new ByteArrayContent(objByte);
                    message.Content.Headers.ContentLength = objByte.Length;
                    message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    message.Content.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}", fileInfo.Name.Replace(" ","_")));
                    message.StatusCode = HttpStatusCode.OK;

                    return message;
                }
                return Content(HttpStatusCode.NoContent, (Descarga)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion
    }
}
