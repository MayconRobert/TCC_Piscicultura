using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site_piscicultura.Controllers
{
    public class PeixeController : Controller
    {
        // GET: Peixe
        public ActionResult Peixes()
        {
            return View();
        }
    }
}