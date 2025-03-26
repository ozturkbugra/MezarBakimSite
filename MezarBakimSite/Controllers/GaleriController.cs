using MezarBakimSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace MezarBakimSite.Controllers
{
    public class GaleriController : Controller
    {
        MezarciEntities db = new MezarciEntities();

        public ActionResult Index(int sayfa = 1)
        {
            Session["Aktif"] = "Galeri";
            ViewBag.Kategoriler = db.Kategorilers.ToList();

            int sayfaBoyutu = 9; // Her sayfada 9 resim gösterilecek
            var liste = db.GaleriViews.OrderByDescending(x => x.ID).ToPagedList(sayfa, sayfaBoyutu);

            return View(liste);
        }
    }
}