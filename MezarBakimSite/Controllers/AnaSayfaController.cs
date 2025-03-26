using MezarBakimSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MezarBakimSite.Controllers
{
    public class AnaSayfaController : Controller
    {
        MezarciEntities db = new MezarciEntities();
        public ActionResult Index()
        {
            Session["Aktif"] = "Ana Sayfa";
            ViewBag.DegerlendirmeSayi = db.SiteYorums.Count();
            
                ViewBag.Degerlendirmeler = db.SiteYorums.ToList();

            return View();
            
        }
    }
}