using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Modulos
{
    public class ModulosController : Controller
    {
        // GET: Modulos
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            return View();
        }

        public PartialViewResult Inicio()
        {
            return PartialView();
        }

        public PartialViewResult CrearModulo()
        {
            return PartialView();
        }
        public PartialViewResult EditarModulo()
        {
            return PartialView();
        }

        public Object Upload(int idUsuario, HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new { CodErr = 1, MsgErr = "No Files", sModulo = "" });
            }
            try
            {
                var user = ProcessMsg.Seguridad.GetUsuario(idUsuario);
                if (user != null)
                {
                    if (!Directory.Exists(Server.MapPath("~/Fuentes")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Fuentes"));
                    }

                    string sRuta = Server.MapPath("~/Fuentes/ModulosXLSX/");

                    if (!Directory.Exists(sRuta))
                    {
                        Directory.CreateDirectory(sRuta);
                    }

                    string sNameFiles = Path.Combine(sRuta, string.Format("{0}.xlsx", user.Id));

                    if (System.IO.File.Exists(sNameFiles))
                    {
                        System.IO.File.Delete(sNameFiles);
                    }
                    System.IO.FileInfo fi = new FileInfo(sNameFiles);
                    if (!fi.Extension.Equals(".xlsx"))
                    {
                        return Json(new { CodErr = 3, MsgErr = "El archivo no es un Excel", sModulo = "" });
                    }

                    file.SaveAs(sNameFiles);

                    if (ProcessMsg.Modulo.AddModulos(user.Id, sNameFiles) > 0)
                    {
                        return Json(new { CodErr = 0, MsgErr = "", sModulo = "OK" });
                    }
                    return Json(new { CodErr = 3, MsgErr = "No se inserto ningun modulo, revise excel", sModulo = "" });
                }
                return Json(new { CodErr = 2, MsgErr = "Usuario no existe", sModulo = "" });
            }
            catch (Exception ex)
            {
                return Json(new { CodErr = 3, MsgErr = ex.Message, sModulo = "" });
            }
        }
        
    }
}