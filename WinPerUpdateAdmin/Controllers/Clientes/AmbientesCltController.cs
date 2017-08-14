using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessMsg;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Configuration;

namespace WinPerUpdateAdmin.Controllers.Clientes
{
    public class AmbientesCltController : Controller
    {
        private char TipoPerfil = 'E';
        // GET: AmbientesClt
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = "AmbientesClt";
                if (Session["token"] == null)
                {
                    return RedirectToAction("Logout", "Home");
                }

                var usuario = ProcessMsg.Seguridad.GetUsuario(int.Parse(Session["token"].ToString()));
                if (usuario == null)
                {
                    return RedirectToAction("Logout", "Home");
                }
                var menus = ProcessMsg.Perfiles.GetMenus(usuario.Id);
                if (menus.Exists(x => x.Link.Contains(ViewBag.Menu)))
                {
                    var perfil = ProcessMsg.Perfiles.GetPerfil(usuario.CodPrf);
                    if (TipoPerfil == perfil.Tipo)
                    {
                        return View();
                    }
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public PartialViewResult Ambientes()
        {
            return PartialView();
        }

        public PartialViewResult Crear()
        {
            return PartialView();
        }
        public Object Upload(int idUsuario, HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new { CodErr = 1, MsgErr = "No Files", sCliente = "" });
            }
            try
            {
                var cliente = ProcessMsg.Cliente.GetClienteUsuario(idUsuario);
                if (cliente != null)
                {
                    string sRuta = ProcessMsg.Utils.GetPathSetting(Server.MapPath("~/Fuentes/") + "AmbientesXlSXClientes/");

                    if (!Directory.Exists(sRuta))
                    {
                        Directory.CreateDirectory(sRuta);
                    }

                    string sNameFiles = Path.Combine(sRuta, string.Format("{0}-{1:ddMMyyyyHHmmss}-{2}", cliente.Id, DateTime.Now, file.FileName));

                    if (System.IO.File.Exists(sNameFiles))
                    {
                        System.IO.File.Delete(sNameFiles);
                    }
                    System.IO.FileInfo fi = new FileInfo(sNameFiles);
                    if (!fi.Extension.Equals(".xlsx"))
                    {
                        return Json(new { CodErr = 3, MsgErr = "El archivo no es una planilla Excel", sCliente = "" });
                    }

                    file.SaveAs(sNameFiles);
                    if (ProcessMsg.Ambiente.AddAmbientesXLSX(cliente.Id,sNameFiles, 1) )
                    {
                        if (ProcessMsg.Ambiente.GetAmbXlSXOk(cliente.Id) == 0)
                        {
                            if (ProcessMsg.Ambiente.AddAmbXLSXtoAmb(cliente.Id) > 0)
                            {
                                return Json(new { CodErr = 0, MsgErr = "", sCliente = "OK" });
                            }
                        }
                        return Json(new { CodErr = 0, MsgErr = "", sCliente = "WARNING" });
                    }
                }
                return Json(new { CodErr = 2, MsgErr = "Cliente no existe", sCliente = "" });
            }
            catch (Exception ex)
            {
                return Json(new { CodErr = 3, MsgErr = ex.Message, sCliente = "" });
            }
        }

        

    }
}