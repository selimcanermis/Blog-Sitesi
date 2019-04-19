using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlogSite.Models;
using System.Web.Helpers;
using System.IO;

namespace _2017Blog.Controllers
{
    public class AdminMakaleController : Controller
    {
        mvcblogDB db = new mvcblogDB();

        // GET: AdminMakale
        public ActionResult Index()
        {
            var makales = db.Makalelers.ToList();
            return View(makales);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.Kategoriid = new SelectList(db.Kategoris, "Kategoriid", "KategoriAdi");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makaleler makale, string etiketler, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {

                WebImage img = new WebImage(Foto.InputStream);
                FileInfo fotoinfo = new FileInfo(Foto.FileName);

                string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                img.Resize(800, 350);
                img.Save("~/Uploads/MakaleFoto/" + newfoto);
                makale.Foto = "/Uploads/MakaleFoto/" + newfoto;




                if (etiketler != null)
                {
                    string[] etiketdizi = etiketler.Split(',');
                    foreach (var i in etiketdizi)
                    {
                        var yenietiket = new Etiket { EtiketAdi = i };
                        db.Etikets.Add(yenietiket);
                        makale.Etikets.Add(yenietiket);
                    }
                }
                makale.Uyeid = Convert.ToInt32(Session["Uyeid"]);
                db.Makalelers.Add(makale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(makale);

        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            var makale = db.Makalelers.Where(m => m.Makaleid == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Kategoris, "Kategoriid", "KategoriAdi", makale.Kategoriid);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase Foto, Makaleler makale)
        {
            try
            {
                var makales = db.Makalelers.Where(m => m.Makaleid == id).SingleOrDefault();
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makales.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makales.Foto = "/Uploads/MakaleFoto/" + newfoto;
                    makales.Baslik = makale.Baslik;
                    makales.icerik = makale.icerik;
                    makales.Kategoriid = makale.Kategoriid;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                ViewBag.Kategoriid = new SelectList(db.Kategoris, "Kategoriid", "KategoriAdi", makale.Kategoriid);
                return View(makale);
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Makalelers.Where(m => m.Makaleid == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var makales = db.Makalelers.Where(m => m.Makaleid == id).SingleOrDefault();
                if (makales == null)
                {
                    return HttpNotFound();
                }

                if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(makales.Foto));
                }
                foreach (var i in makales.Yorums.ToList())
                {
                    db.Yorums.Remove(i);
                }
                foreach (var i in makales.Etikets.ToList())
                {
                    db.Etikets.Remove(i);
                }
                db.Makalelers.Remove(makales);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
