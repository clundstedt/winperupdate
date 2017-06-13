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
        private char TipoPerfil = 'I';
        // GET: Modulos
        public ActionResult Index()
        {
            try
            {
                ViewBag.Menu = "Modulos";
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
                    if (!Directory.Exists(ProcessMsg.Utils.GetPathSetting(Server.MapPath("~/Fuentes"))))
                    {
                        Directory.CreateDirectory(ProcessMsg.Utils.GetPathSetting(Server.MapPath("~/Fuentes")));
                    }

                    string sRuta = ProcessMsg.Utils.GetPathSetting(Server.MapPath("~/Fuentes/") +  "ModulosXLSX/");

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