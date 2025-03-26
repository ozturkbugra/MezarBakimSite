using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MezarBakimSite.Controllers
{
    public class HakkimizdaController : Controller
    {
                
        public ActionResult Index()
        {
            Session["Aktif"] = "Hakkımızda";
            return View();
        }
    }
}