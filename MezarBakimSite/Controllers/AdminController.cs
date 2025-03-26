using MezarBakimSite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MezarBakimSite.Controllers
{
    public class AdminController : Controller
    {
        MezarciEntities db = new MezarciEntities();
        public ActionResult Login()
        {
            return View();
        }

       [HttpPost]
        public ActionResult Login(Admin admin, string Parola)
        {
            var p = Crypto.Hash(Parola, "MD5");

            var k = db.Admins.Where(x => x.KullaniciAdi == admin.KullaniciAdi && x.Parola == p).FirstOrDefault();

            if (k != null)
            {
                Session["AdminID"] = k.ID;
                Session["KullaniciAdi"] = k.KullaniciAdi;
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Uyari = "Kullanıcı Adı veya Şifre Hatalı";
            return View();
        }
        public ActionResult Index()
        {
            return View();
        } 
        public ActionResult Degerlendirmeler()
        {
            ViewBag.List = db.SiteYorums.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult DegerlendirmeEkle(SiteYorum siteYorum, HttpPostedFileBase ResimURL)
        {

            WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme

            FileInfo imginfo = new FileInfo(ResimURL.FileName);

            string name = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma

            img.Save("~/Resim/" + name);
            siteYorum.ResimURL = "/resim/" + name;

            db.SiteYorums.Add(siteYorum);
            db.SaveChanges();
            TempData["Basarili"] = "Başarıyla Eklendi";
            return RedirectToAction("Degerlendirmeler");
        }

        public ActionResult DegerlendirmeSil(int id)
        {
            SiteYorum siteYorum = db.SiteYorums.FirstOrDefault(x => x.ID == id);
            if (System.IO.File.Exists(Server.MapPath(siteYorum.ResimURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
            {
                System.IO.File.Delete(Server.MapPath(siteYorum.ResimURL));
            }
            db.SiteYorums.Remove(siteYorum);
            db.SaveChanges();
            return RedirectToAction("Degerlendirmeler");
        }
        public ActionResult Kategoriler()
        {
            ViewBag.Liste = db.Kategorilers.ToList();
            return View();
        }
        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategoriler kategoriler)
        {
            db.Kategorilers.Add(kategoriler);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }


        public ActionResult KategoriDuzenle(int id)
        {
            ViewBag.k = db.Kategorilers.FirstOrDefault(x => x.ID == id);
            return View();
        }

        [HttpPost]
        public ActionResult KategoriDuzenle(int id, Kategoriler kategoriler)
        {
            var bul = db.Kategorilers.FirstOrDefault(x => x.ID == id);
            bul.KategoriAdi = kategoriler.KategoriAdi;
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public ActionResult KategoriSil(int id)
        {
            var bul = db.Kategorilers.FirstOrDefault(x => x.ID == id);
            var varmi = db.Galeris.FirstOrDefault(x => x.KategoriID == id);

            if(varmi == null)
            {
                db.Kategorilers.Remove(bul);
                db.SaveChanges();
            }

            return RedirectToAction("Kategoriler");
        }

        public ActionResult Logout()
        {
            Session["AdminID"] = null;
            Session["KullaniciAdi"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }


        public ActionResult Resimler()
        {
            ViewBag.Liste = db.GaleriViews.OrderByDescending(x => x.ID).ToList();
            ViewBag.Kategoriler = db.Kategorilers.OrderBy(x=> x.KategoriAdi).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ResimEkle(Galeri galeri, HttpPostedFileBase ResimURL)
        {

            WebImage img = new WebImage(ResimURL.InputStream); //bu ikisi resim ekleme

            FileInfo imginfo = new FileInfo(ResimURL.FileName);

            string name = Guid.NewGuid().ToString() + imginfo.Extension; //resim adlandırma

            img.Save("~/Resim/" + name);
            galeri.ResimURL = "/resim/" + name;

            db.Galeris.Add(galeri);
            db.SaveChanges();
            TempData["Basarili"] = "Başarıyla Eklendi";
            return RedirectToAction("Resimler");
        }


        [HttpPost]
        public ActionResult TopluResimEkle(Galeri galeri, IEnumerable<HttpPostedFileBase> ResimURL)
        {
            if (ResimURL != null)
            {
                foreach (var file in ResimURL)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        WebImage img = new WebImage(file.InputStream);
                        FileInfo imginfo = new FileInfo(file.FileName);

                        string name = Guid.NewGuid().ToString() + imginfo.Extension; // Benzersiz isimlendirme
                        string path = "~/Resim/" + name;

                        img.Save(path);

                        // Yeni galeri nesnesi oluşturuluyor
                        Galeri yeniGaleri = new Galeri
                        {
                            ResimURL = "/resim/" + name,
                            KategoriID = galeri.KategoriID
                        };

                        db.Galeris.Add(yeniGaleri);
                    }
                }

                db.SaveChanges();
                TempData["Basarili"] = "Resimler başarıyla eklendi";
            }
            else
            {
                TempData["Hata"] = "Lütfen en az bir resim seçin";
            }

            return RedirectToAction("Resimler");
        }



        public ActionResult ResimSil(int id)
        {
            Galeri galeri = db.Galeris.FirstOrDefault(x => x.ID == id);
            if (System.IO.File.Exists(Server.MapPath(galeri.ResimURL))) //daha önce kaydettiğimiz dosya varsa silme kodu
            {
                System.IO.File.Delete(Server.MapPath(galeri.ResimURL));
            }
            db.Galeris.Remove(galeri);
            db.SaveChanges();
            return RedirectToAction("Resimler");
        }

        public ActionResult Yetkililer()
        {
            ViewBag.Yetkililer = db.Admins.OrderByDescending(x => x.ID).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult YetkiliEkle(Admin admin, string Parola)
        {
          
            var varmi = db.Admins.Where(x => x.KullaniciAdi == admin.KullaniciAdi).FirstOrDefault();
            if (varmi != null)
            {
                TempData["Hata"] = "Bu Admin Zaten Kayıtlı";
                return RedirectToAction("Yetkililer", "Admin");
            }

            var p = Crypto.Hash(Parola, "MD5");
            admin.Parola = p;

            db.Admins.Add(admin);
            db.SaveChanges();
            TempData["Basarili"] = "Başarıyla Eklendi";
            return RedirectToAction("Yetkililer", "Admin");
        }

        public ActionResult YetkiliSil(int id)
        {
            var admin = db.Admins.Where(x => x.ID == id).FirstOrDefault();
            var sayi = db.Admins.Count();

            if (sayi >= 2)
            {
                db.Admins.Remove(admin);
                db.SaveChanges();
                TempData["Basarili"] = "Başarıyla Silindi";
                return RedirectToAction("Yetkililer", "Admin");
            }

            TempData["Hata"] = "Tüm Adminleri Silemezsiniz";

            return RedirectToAction("Yetkililer", "Admin");
        }




        public ActionResult Password()
        {
         
            return View();
        }

        [HttpPost]
        public ActionResult Password(string eskiparola, string Parola, string Parola2)
        {
            int adminid = Convert.ToInt32(Session["AdminID"]);
            var a = db.Admins.Where(x => x.ID == adminid).FirstOrDefault();
            var eskisifre = Crypto.Hash(eskiparola, "MD5");
            if (a.Parola == eskisifre)
            {
                if (Parola == Parola2)
                {

                    a.Parola = Crypto.Hash(Parola, "MD5");
                    db.SaveChanges();
                    TempData["Basarili"] = "Şifreniz Başarıyla Güncellendi.";

                }
                else
                {
                    TempData["Hata"] = "Şifreleriniz Birbiriyle Uyuşmuyor.";

                }
            }
            else
            {
                TempData["Hata"] = "Eski Şifreniz Hatalı.";
            }

            return View();

        }
    }
}