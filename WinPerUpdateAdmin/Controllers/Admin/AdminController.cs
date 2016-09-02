using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessMsg;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace WinPerUpdateAdmin.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Login = "Admin";
            return View();
        }

        public PartialViewResult Versiones()
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
    }
}