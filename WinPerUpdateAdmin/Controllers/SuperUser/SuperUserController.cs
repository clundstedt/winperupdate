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
            return View();
        }

        public PartialViewResult Configuracion()
        {
            return PartialView();
        }
    }
}