using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlogSite.Models;
using PagedList;
using PagedList.Mvc;

namespace MvcBlogSite.Controllers
{
    public class HomeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Home
        public ActionResult Index(int Page=1)
        {
            var makale = db.Makalelers.OrderByDescending(m => m.Kategori.Kategoriid).ToPagedList(Page,3);
            return View(makale);
        }
        public ActionResult BlogAra(string Ara = null)
        {
            var aranan = db.Makalelers.Where(m => m.Baslik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(m => m.Tarih));
        }
        public ActionResult SonYorumlar()
        {
            return View(db.Yorums.OrderByDescending(y=>y.Yorumid).Take(5));
        }
        public ActionResult TopMakaleler()
        {
            return View(db.Makalelers.OrderByDescending(m => m.Okunma).Take(5));
        }
        public ActionResult KategoriMakale(int id)
        {
            var makaleler = db.Makalelers.Where(m => m.Makaleid == id).ToList();
            return View(makaleler);
        }
        public ActionResult MakaleDetay(int id)
        {
            var Makale = db.Makalelers.Where(m => m.Makaleid == id).SingleOrDefault();
            if (Makale == null)
            {
                return HttpNotFound();
            }

            return View(Makale);
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult KategoriPartial()
        {
            return View(db.Kategoris.ToList());
        }
        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            var uyeid = Session["uyeid"];
            if (yorum != null)
            {
                db.Yorums.Add(new Yorum { Uyeid = Convert.ToInt32(uyeid), Makaleid = Makaleid, icerik = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];
            var yorum = db.Yorums.Where(y => y.Yorumid == id).SingleOrDefault();
            var makale = db.Makalelers.Where(m => m.Makaleid == yorum.Makaleid).SingleOrDefault();
            if (yorum.Uyeid == Convert.ToInt32(uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.Makaleid });
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult OkunmaArttir(int Makaleid)
        {
            var makale = db.Makalelers.Where(m => m.Makaleid == Makaleid).SingleOrDefault();
            makale.Okunma += 1;
            db.SaveChanges();
            return View();
        }
    }
}