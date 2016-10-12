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

namespace WinPerUpdateAdmin.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            ViewBag.Menu = "Admin";
            return View();
        }

        public PartialViewResult Versiones()
        {
            return PartialView();
        }

        public PartialViewResult Clientes()
        {
            return PartialView();
        }

        public PartialViewResult CrearVersion()
        {
            return PartialView();
        }

        public PartialViewResult CrearComponente()
        {
            return PartialView();
        }

        public PartialViewResult EditComponente()
        {
            return PartialView();
        }

        public PartialViewResult UploadFile()
        {
            return PartialView();
        }

        public PartialViewResult PubParcial()
        {
            return PartialView();
        }

        public Object Upload(int idVersion, HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new { CodErr = 1, MsgErr = "No Files", sVersion = "" });
            }
            try
            {
                var version = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (version == null) return Json(new { CodErr = 2, MsgErr = "Version no existe", sVersion = "" });

                string sRuta = Server.MapPath("~/Uploads/") + version.Release;

                if (!Directory.Exists(sRuta))
                {
                    Directory.CreateDirectory(sRuta);
                }
                string sNameFiles = Path.Combine(sRuta, file.FileName);

                if (System.IO.File.Exists(sNameFiles))
                {
                    System.IO.File.Delete(sNameFiles);
                }

                file.SaveAs(sNameFiles);

                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(sNameFiles);

                return Json(new { CodErr = 0, MsgErr = "", sVersion = myFileVersionInfo.FileVersion ?? "S/I" });
            }
            catch (Exception ex)
            {
                return Json(new { CodErr = 3, MsgErr = ex.Message, sVersion = "" });
            }
        }

        [HttpPost]
        public Object GenerarVersion (FormCollection form)
        {
            try
            {
                int idVersion = int.Parse(form["idVersion"]);
                var version = ProcessMsg.Version.GetVersiones(null).SingleOrDefault(x => x.IdVersion == idVersion);
                if (version == null) return Json(new { CodErr = 2, MsgErr = "Version no existe", Output = "" });

                string sRuta = Server.MapPath("~/Uploads/") + version.Release;
                if (!sRuta.EndsWith("\\")) sRuta += @"\";
                string sFile = "WP" + version.Release.Replace(".", "") + string.Format("{0:yyyyMMddhhhhmmss}", DateTime.Now);

                if (ProcessMsg.Version.GenerarInstalador(idVersion, sFile, sRuta) > 0)
                {
                    string Command = ConfigurationManager.AppSettings["pathGenSetup"];
                    string argument = "\"" + sRuta + sFile + ".iss\"";

                    System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(Command, argument);

                    // Indicamos que la salida del proceso se redireccione en un Stream
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;

                    //Indica que el proceso no despliegue una pantalla negra (El proceso se ejecuta en background)
                    procStartInfo.CreateNoWindow = false;

                    //Inicializa el proceso
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();

                    //Consigue la salida de la Consola(Stream) y devuelve una cadena de texto
                    string result = proc.StandardOutput.ReadToEnd();

                    return Json(new { CodErr = 0, MsgErr = result, Output = sFile + ".exe" });
                }
                return Json(new { CodErr = 1, MsgErr = "No pudo generar archivo de setup", Output = "" });

            }
            catch (Exception ex)
            {
                return Json(new { CodErr = 3, MsgErr = ex.Message, Output = "" });
            }
        }

    }
}