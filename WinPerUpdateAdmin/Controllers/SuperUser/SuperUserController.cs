using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinPerUpdateAdmin.Controllers.SuperUser
{
    public class SuperUserController : Controller
    {
        // GET: SuperUser
        public ActionResult Index()
        {
            if (Session["token"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            return View();
        }

        public PartialViewResult Configuracion()
        {
            return PartialView();
        }
    }
}