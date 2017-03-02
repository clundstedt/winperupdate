using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.Descargas
{
    public class DescargasController : Controller
    {
        // GET: Descargas
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            return View();
        }

        public PartialViewResult Descargas()
        {
            return PartialView();
        }
    }
}