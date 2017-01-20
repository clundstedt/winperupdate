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
        // GET: AmbientesClt
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            var usuario = ProcessMsg.Seguridad.GetUsuario(int.Parse(Session["token"].ToString()));
            if (usuario == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            ViewBag.Menu = "AmbientesClt";

            return View();
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
                    if (ProcessMsg.Ambiente.AddAmbientesXLSX(cliente.Id,sNameFiles, "Ambientes") )
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