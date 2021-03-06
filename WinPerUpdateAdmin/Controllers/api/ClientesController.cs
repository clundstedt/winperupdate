﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace WinPerUpdateAdmin.Controllers.api
{
    public class ClientesController : ApiController
    {

        #region structs
        struct anio
        {
            public int UltimoDigito { get; set; }
            public int Ano { get; set; }
        }

        struct JsonDataUsuario
        {
            public int CodErr { get; set; }
            public ProcessMsg.Model.UsuarioBo Usuario { get; set; }
        }
        #endregion

        #region get

        [Route("api/getClienteNoVigente/{id:int}")]
        [HttpGet]
        [Authorize]
        public Object GetClienteNoVigente(int id)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var cl = ProcessMsg.Cliente.GetClienteNoVigente(id);
                return Content(HttpStatusCode.OK, cl.Count > 0 ? cl.ElementAt(0) : (object)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Bienvenida/Usuario/{idUsuario:int}")]
        [HttpGet]
        [Authorize]
        public Object EnviarBienvenidaUsuario(int idUsuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Seguridad.GetUsuario(idUsuario);
                if (obj == null)
                {
                    return Content(HttpStatusCode.Created, new JsonDataUsuario { CodErr = 2, Usuario = null });//No existe el usuario
                }
                obj = ProcessMsg.Seguridad.GetUsuario(obj.Persona.Mail);
                if (obj == null)
                {
                    return Content(HttpStatusCode.Created, new JsonDataUsuario { CodErr = 2, Usuario = null });//No existe el usuario
                }
                var clt = ProcessMsg.Cliente.GetClienteUsuario(obj.Id);
                if (clt != null)
                {
                    var baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                    var htmlMailer = File.ReadAllText(Path.Combine(ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes")), "Mailer", "mailer.html"));
                    var uriImg = string.Format("{0}/{1}/{2}/{3}", baseUrl, ProcessMsg.Utils.GetPathSetting("Fuentes"), "Mailer", "images");
                    var Fecha = DateTime.Now.ToLongDateString();
                    var Nombre = obj.Persona.Nombres + " " + obj.Persona.Apellidos;
                    var Link = ConfigurationManager.AppSettings["link"]; 
                    var Correo = obj.Persona.Mail;
                    var Password = ProcessMsg.Utils.DesEncriptar(obj.Clave);
                    var Licencia = clt.NroLicencia;

                    htmlMailer = htmlMailer.Replace("@uriImg", uriImg);
                    htmlMailer = htmlMailer.Replace("@Fecha", Fecha);
                    htmlMailer = htmlMailer.Replace("@Nombre", Nombre);
                    htmlMailer = htmlMailer.Replace("@Link", Link);
                    htmlMailer = htmlMailer.Replace("@Correo", Correo);
                    htmlMailer = htmlMailer.Replace("@Password", Password);
                    htmlMailer = htmlMailer.Replace("@Licencia", Licencia);



                    if (ProcessMsg.Utils.SendMail(htmlMailer, "Activación cuenta WinperUpdate", obj.Persona.Mail))
                    {
                        return Content(HttpStatusCode.Created, new JsonDataUsuario { CodErr = 0, Usuario = obj });//Se envio el correo
                    }

                    return Content(HttpStatusCode.Created, new JsonDataUsuario { CodErr = 1, Usuario = obj });//No se pudo enviar el correo
                }
                return Content(HttpStatusCode.Created, new JsonDataUsuario { CodErr = 3, Usuario = null });//No existe el cliente para el usuario
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/GenCorrelativo/{MesCon}/{Folio:int}")]
        [HttpGet]
        [Authorize]
        public Object GenCorrelativo(int Folio, string MesCon)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Cliente.GetCorrelativo(Folio, MesCon);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/GetAnios")]
        [HttpGet]
        [Authorize]
        public Object GetAnios()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                List<anio> lista = new List<anio>();
                for (int i = 1990; i <= DateTime.Now.Year; i++)
                {
                    lista.Add(new anio
                    {
                        UltimoDigito = int.Parse(i.ToString().Last().ToString()),
                        Ano = i
                    });
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/Clientes/{idCliente:int}/PDF")]
        [HttpGet]
        [Authorize]
        public Object GetInformeClientes(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var Cliente = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == idCliente);
                if (Cliente != null)
                {
                    string pdf = string.Format("{0}", ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes")));
                    #region Creador de directorio y archivo
                    if (!Directory.Exists(pdf))
                    {
                        Directory.CreateDirectory(pdf);
                    }
                    pdf = Path.Combine(pdf, "PDF");
                    if (!Directory.Exists(pdf))
                    {
                        Directory.CreateDirectory(pdf);
                    }
                    pdf = Path.Combine(pdf, "Clientes");
                    if (!Directory.Exists(pdf))
                    {
                        Directory.CreateDirectory(pdf);
                    }
                    pdf = Path.Combine(pdf, string.Format("Cliente{0:ddMMyyyyHHmmss}_{1}.pdf", DateTime.Now, idCliente));
                    #endregion

                    #region Creando PDF
                    Document document = new Document();

                    PdfWriter.GetInstance(document,
                                  new FileStream(pdf,
                                         FileMode.OpenOrCreate));
                    document.Open();
                    #region Titulo
                    var tituloPdf = new Paragraph(
                            new Chunk(Cliente.Nombre,
                            FontFactory.GetFont("ARIAL", 16, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD)));
                    tituloPdf.Alignment = Element.ALIGN_CENTER;
                    document.Add(tituloPdf);
                    var rutPdf = new Paragraph(
                            new Chunk(string.Format("{0}",Cliente.RutFmt),
                            FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD)));
                    rutPdf.Alignment = Element.ALIGN_CENTER;
                    document.Add(rutPdf);
                    #endregion
                    #region Fecha Generacion
                    var fechaCreacion = new FileInfo(pdf).CreationTime.ToLongDateString();
                    var fechaGeneracion = new Paragraph(
                            new Chunk(fechaCreacion.ElementAt(0).ToString().ToUpper()+ fechaCreacion.Substring(1, fechaCreacion.Length-1),
                            FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.ITALIC)));
                    fechaGeneracion.Alignment = Element.ALIGN_CENTER;
                    document.Add(fechaGeneracion);
                    #endregion
                    #region Datos Cliente
                    document.Add(new Paragraph(new Chunk("Datos Cliente", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD))));
                    document.Add(new Paragraph(new Chunk(string.Format("Dirección: {0}",Cliente.Direccion), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("Región: {0}",Cliente.Comuna.Region.NomRgn), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("Comuna: {0}",Cliente.Comuna.NomCmn), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("N° Cliente: {0}",Cliente.NumFolio), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("Estado de Mantención: {0}",Cliente.EstMtcFmt), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("Mes Inicio de Contrato: {0}",Cliente.MesIniFmt), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("N° Trabajadores Contratados: {0}",Cliente.NroTrbc), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("N° Trabajadores Honorarios: {0}",Cliente.NroTrbh), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("Máxima Cantidad de Usuarios Permitidos: {0}",Cliente.NroUsr), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    document.Add(new Paragraph(new Chunk(string.Format("N° Licencia: {0}",Cliente.NroLicencia), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.NORMAL))));
                    #endregion
                    #region Tabla Modulos
                    document.Add(new Paragraph(new Chunk("Módulos Contratados", FontFactory.GetFont("ARIAL",12, iTextSharp.text.Font.BOLD))));
                    document.Add(new Paragraph(" "));
                    PdfPTable tbl = ProcessMsg.Utils.GenerarTablaPDF(95, Element.ALIGN_CENTER, ProcessMsg.Cliente.GetModulosClientePDF(Cliente.Id));
                    document.Add(tbl);
                    #endregion
                    #region Tabla Versiones Instaladas
                    document.Add(new Paragraph(new Chunk("Versiones Instaladas", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD))));
                    document.Add(new Paragraph(" "));
                    PdfPTable tblv = ProcessMsg.Utils.GenerarTablaPDF(95, Element.ALIGN_CENTER, ProcessMsg.Cliente.GetVersionToClientePDF(Cliente.Id));
                    document.Add(tblv);
                    #endregion

                    document.Close();
                    #endregion

                    #region Descarga de PDF
                    Byte[] objByte = System.IO.File.ReadAllBytes(pdf);
                    if (objByte == null)
                    {
                        return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                    }
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);

                    message.Content = new ByteArrayContent(objByte);
                    message.Content.Headers.ContentLength = objByte.Length;
                    message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                    message.Content.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}", new FileInfo(pdf).Name));
                    message.StatusCode = HttpStatusCode.OK;

                    return message;
                    #endregion

                }
                return null;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/Clientes/PDF")]
        [HttpGet]
        [Authorize]
        public Object GetInformeClientes()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                string pdf = string.Format("{0}", ProcessMsg.Utils.GetPathSetting(HttpContext.Current.Server.MapPath("~/Fuentes")));
                #region Creador de directorio y archivo
                if (!Directory.Exists(pdf))
                {
                    Directory.CreateDirectory(pdf);
                }
                pdf = Path.Combine(pdf, "PDF");
                if (!Directory.Exists(pdf))
                {
                    Directory.CreateDirectory(pdf);
                }
                pdf = Path.Combine(pdf, "Clientes");
                if (!Directory.Exists(pdf))
                {
                    Directory.CreateDirectory(pdf);
                }
                pdf = Path.Combine(pdf, string.Format("InformeClientes{0:ddMMyyyyHHmmss}.pdf", DateTime.Now));

                #endregion

                #region Creando PDF
                Document document = new Document();

                PdfWriter.GetInstance(document,
                              new FileStream(pdf,
                                     FileMode.OpenOrCreate));
                document.Open();
                #region Titulo
                var tituloPdf = new Paragraph(
                        new Chunk("Informe Clientes",
                        FontFactory.GetFont("ARIAL", 16, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD)));
                tituloPdf.Alignment = Element.ALIGN_CENTER;
                document.Add(tituloPdf);
                #endregion
                #region Fecha Generacion
                var fechaCreacion = new FileInfo(pdf).CreationTime.ToLongDateString();
                var fechaGeneracion = new Paragraph(
                        new Chunk(fechaCreacion.ElementAt(0).ToString().ToUpper() + fechaCreacion.Substring(1, fechaCreacion.Length - 1),
                        FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.ITALIC)));
                fechaGeneracion.Alignment = Element.ALIGN_CENTER;
                document.Add(fechaGeneracion);
                document.Add(new Paragraph(" "));
                #endregion
                #region Tabla
                PdfPTable tbl = ProcessMsg.Utils.GenerarTablaPDF(95, Element.ALIGN_CENTER, ProcessMsg.Cliente.GetClientesPDF());
            
                document.Add(tbl);
                #endregion

                document.Close();
                #endregion

                #region Descarga de PDF
                Byte[] objByte = System.IO.File.ReadAllBytes(pdf);
                if (objByte == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ByteArrayContent)null);
                }
                
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Created);

                message.Content = new ByteArrayContent(objByte);
                message.Content.Headers.ContentLength = objByte.Length;
                message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                //message.Content.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}", new FileInfo(pdf).Name));
                message.StatusCode = HttpStatusCode.OK;
                return message;
                
                /*
                ResponseBuilder response = Response.ok(new TemporaryFileInputStream(reportFile));
                response.header("Content-Disposition", "attachment; filename=" + pdf);
                response.header("Content-Type", "application/pdf");
                response.header("Access-Control-Expose-Headers", "x-filename");
                response.header("x-filename", pdf);
                return response.build();
                */
                #endregion
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/TrabPlantas")]
        [HttpGet]
        [Authorize]
        public Object GetTrabPlantas()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var lista = ProcessMsg.Cliente.GetTrabPlantas();
                return lista.OrderBy(x => x.HastaFmt);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/TrabHonorarios")]
        [HttpGet][Authorize]
        public Object GetTrabHonorarios()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Cliente.GetTrabHonorarios();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Cliente/{idCliente:int}/VersionesInstaladas")]
        [HttpGet][Authorize]
        public Object GetVersionesClientes(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                return ProcessMsg.Version.GetVersionesToCliente(idCliente);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        [Route("api/Cliente/{idCliente:int}/ModulosWinper")]
        [HttpGet][Authorize]
        public Object GetModulosCliente(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == idCliente);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return ProcessMsg.Cliente.GetClientesHasModulo(idCliente);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes")]
        [HttpGet][Authorize]
        public Object Get()
        {
            try
            {
                var u = HttpContext.Current.Request.Url;
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetClientes();
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj.OrderBy(x => x.Nombre);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/{idCliente:int}")]
        [HttpGet][Authorize]
        public Object GetCliente(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == idCliente);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/{idCliente:int}/Versiones")]
        [HttpGet][Authorize]
        public Object GetVersionesCliente(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetVersiones(idCliente, null);
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

        [Route("api/Clientes/{idCliente:int}/Usuarios")]
        [HttpGet][Authorize]
        public Object GetUsuariosCliente(int idCliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetUsuarios(idCliente);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.UsuarioBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Clientes/{idCliente:int}/Usuarios/{idUsuario:int}")]
        [HttpGet][Authorize]
        public Object GetUsuariosCliente(int idCliente, int idUsuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetUsuarios(idCliente).SingleOrDefault(x => x.Id == idUsuario);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.UsuarioBo)null);
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Region")]
        [HttpGet][Authorize]
        public Object GetRegiones()
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetRegiones();
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.RegionBo)null);
                }

                return obj.OrderBy(x => x.NomRgn);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Region/{idRgn:int}/Comunas")]
        [HttpGet][Authorize]
        public Object GetComunas(int idRgn)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var obj = ProcessMsg.Cliente.GetComunas(idRgn);
                if (obj == null)
                {
                    return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.ComunaBo)null);
                }

                return obj.OrderBy(x => x.NomCmn);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        [Route("api/Key/{Folio:int}/{MesCon}/{Correlativo:int}/{EstMtc:int}/{MesIni}/{NroTrbc}/{NroTrbh}/{NroUsr}")]
        [HttpGet][Authorize]
        public Object GetKeyCliente(int Folio, string MesCon, int Correlativo, int EstMtc, string MesIni, string NroTrbc, string NroTrbh, string NroUsr)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var key = ProcessMsg.Utils.GenerarLicencia(Folio, MesCon, Correlativo, EstMtc, MesIni, NroTrbc, NroTrbh, NroUsr);
                return Content(HttpStatusCode.OK, key); ;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        #endregion

        #region post
        [Route("api/Cliente/{idCliente:int}/Modulos")]
        [HttpPost][Authorize]
        public Object PostModulosClientes(int idCliente,[FromBody]List<ProcessMsg.Model.ModuloBo> modulos)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                var cliente = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == idCliente);
                if (cliente != null)
                {
                    if (modulos.Count > 0)
                    {
                        string vant = "";
                        string vnue = "";
                        var modsCl = ProcessMsg.Cliente.GetClientesHasModulo(idCliente);

                        foreach (var m in modsCl)
                        {
                            vant += string.Format("Modulo={0}|", m.NomModulo);
                        }
                        foreach (var m in modulos)
                        {
                            vnue += string.Format("Modulo={0}|", m.NomModulo);
                        }
                        ProcessMsg.Bitacora.AddBitacora("Cliente", vant, vnue, 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), cliente.Bitacora('?'));



                        var idModulos = modulos.Select(x => x.idModulo).ToArray();
                        if (ProcessMsg.Cliente.AddClientesHasModulos(idCliente, idModulos))
                        {
                            return Content(HttpStatusCode.OK, true);
                        }
                    }
                    return Content(HttpStatusCode.Created,false);
                }
                return Content(HttpStatusCode.NotFound, (ProcessMsg.Model.ClienteBo)null);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Clientes")]
        [HttpPost][Authorize]
        public Object Post([FromBody]ProcessMsg.Model.ClienteBo cliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created); 

                if (cliente.Rut == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Dv.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Nombre.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Comuna == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    cliente.Estado = 'V';
                    //registro en bitacora
                    ProcessMsg.Bitacora.AddBitacora("Cliente"
                        , null
                        , cliente.Bitacora('I')
                        , 'I'
                        , DateTime.Now
                        , int.Parse(HttpContext.Current.Session["token"].ToString())
                        , cliente.Bitacora('?'));
                    
                    var obj = ProcessMsg.Cliente.Add(cliente);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Clientes/{idCliente:int}/Usuarios")]
        [HttpPost][Authorize]
        public Object PostUsuario(int idCliente, [FromBody]ProcessMsg.Model.UsuarioBo usuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (usuario.Persona.Nombres == null || usuario.Persona.Apellidos == null || usuario.Persona.Mail == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (usuario.CodPrf == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var clt = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == idCliente);
                    if (clt != null)
                    {
                        if (usuario.Clave == null)
                        {
                            usuario.Clave = ProcessMsg.Utils.Encriptar(ProcessMsg.Utils.RandomString(10));
                        }
                        var persona = ProcessMsg.Seguridad.AddPersona(usuario.Persona);
                        if (persona == null)
                        {
                            response.StatusCode = HttpStatusCode.Accepted;
                        }
                        else
                        {
                            usuario.Persona = persona;
                            usuario.Cliente = clt;
                            ProcessMsg.Bitacora.AddBitacora("Usuario", null, usuario.Bitacora('I'), 'I', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), usuario.Bitacora('?'));
                            var obj = ProcessMsg.Seguridad.AddUsuarioCliente(usuario);
                            if (obj == null)
                            {
                                return Content(HttpStatusCode.Accepted, (ProcessMsg.Model.VersionBo)null);
                            }

                            var res = ProcessMsg.Cliente.AddUsuario(idCliente, obj.Id);
                            if (res == 0)
                            {
                                return Content(HttpStatusCode.Accepted, (ProcessMsg.Model.VersionBo)null);
                            }

                            return Content(HttpStatusCode.Created, obj);
                        }
                    }
                }

                return Content(HttpStatusCode.Accepted, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        #endregion

        #region put
        [Route("api/Clientes/{id:int}")]
        [HttpPut][Authorize]
        public Object Put(int id, [FromBody]ProcessMsg.Model.ClienteBo cliente)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (id <= 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Rut == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Dv.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Nombre.ToString() == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (cliente.Comuna == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    cliente.Id = id;
                    //registro en bitacora
                    var cltAnt = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == id);
                    if (cltAnt != null)
                    {
                        ProcessMsg.Bitacora.AddBitacora("Cliente"
                            , cltAnt.Bitacora('U')
                            , cliente.Bitacora('U')
                            , 'U'
                            , DateTime.Now
                            , int.Parse(HttpContext.Current.Session["token"].ToString())
                            , cltAnt.Bitacora('?'));
                    }

                    var obj = ProcessMsg.Cliente.Update(id, cliente);
                    if (obj == null)
                    {
                        response.StatusCode = HttpStatusCode.Accepted;
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, obj);
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        
        [Route("api/Clientes/{id:int}/Usuarios/{idUsuario:int}")]
        [HttpPut][Authorize]
        public Object Put(int id, int idUsuario, [FromBody]ProcessMsg.Model.UsuarioBo usuario)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);

                if (usuario.Persona.Nombres == null || usuario.Persona.Apellidos == null || usuario.Persona.Mail == null)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else if (usuario.CodPrf == 0)
                    response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    var userAnt = ProcessMsg.Seguridad.GetUsuario(idUsuario);
                    if (userAnt != null)
                    {
                        var persona = ProcessMsg.Seguridad.UpdPersona(usuario.Persona);
                        if (persona == null)
                        {
                            response.StatusCode = HttpStatusCode.Accepted;
                        }
                        else
                        {
                            usuario.Persona = persona;
                            usuario.Id = idUsuario;
                            ProcessMsg.Bitacora.AddBitacora("Usuario", userAnt.Bitacora('U'), usuario.Bitacora('U'), 'U', DateTime.Now, int.Parse(HttpContext.Current.Session["token"].ToString()), userAnt.Bitacora('?'));
                            ProcessMsg.Seguridad.UpdUsuario(usuario);

                            var obj = ProcessMsg.Cliente.GetUsuarios(id).SingleOrDefault(x => x.Id == idUsuario);
                            if (obj == null)
                            {
                                response.StatusCode = HttpStatusCode.Accepted;
                            }
                            return Content(response.StatusCode, obj);
                        }
                    }
                }

                return Content(response.StatusCode, (ProcessMsg.Model.VersionBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }

        #endregion

        #region delete
        [Route("api/Clientes/Vigente")]
        [HttpDelete][Authorize]
        public Object Delete(int id, char est, string motivo)
        {
            try
            {
                if (HttpContext.Current.Session["token"] == null) return Redirect(Request.RequestUri.GetLeftPart(UriPartial.Authority));

                //registro en bitacora
                var cl = ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == id);
                if (cl != null)
                {
                    if (est == 'C')
                    {
                        if (ProcessMsg.Cliente.AddClienteNoVigente(DateTime.Now, motivo, id, int.Parse(HttpContext.Current.Session["token"].ToString())) == 1)
                        {
                            if (ProcessMsg.Cliente.Delete(id, est) == 1)
                            {
                                ProcessMsg.Bitacora.AddBitacora("Cliente"
                                                               , cl.Bitacora(est == 'C' ? 'N' : 'V')
                                                               , null, (est == 'C' ? 'N' : 'V')
                                                               , DateTime.Now
                                                               , int.Parse(HttpContext.Current.Session["token"].ToString())
                                                               , cl.Bitacora('?'));
                                return Content(HttpStatusCode.Created, (ProcessMsg.Model.ClienteBo)ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == id));
                            }
                        }
                    }
                    else
                    {
                        if (ProcessMsg.Cliente.Delete(id, est) == 1)
                        {
                            ProcessMsg.Bitacora.AddBitacora("Cliente"
                                                            , cl.Bitacora(est == 'C' ? 'N' : 'V')
                                                            , null, (est == 'C' ? 'N' : 'V')
                                                            , DateTime.Now
                                                            , int.Parse(HttpContext.Current.Session["token"].ToString())
                                                            , cl.Bitacora('?'));
                            return Content(HttpStatusCode.Created, (ProcessMsg.Model.ClienteBo)ProcessMsg.Cliente.GetClientes().SingleOrDefault(x => x.Id == id));
                        }
                    }
                }
                return Content(HttpStatusCode.BadRequest, (ProcessMsg.Model.ClienteBo)null);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message));
            }
        }
        #endregion
    }
}
