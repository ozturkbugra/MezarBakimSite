using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MezarBakimSite.Controllers
{
    public class FiyatListesiController : Controller
    {
        
        public ActionResult Index()
        {
            Session["Aktif"] = "Fiyat Listesi";
            return View();
        }
    }
}