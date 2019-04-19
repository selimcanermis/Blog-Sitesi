using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlogSite.Models;

namespace MvcBlogSite.Controllers
{
    public class AdminController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.MakaleSayisi = db.Makalelers.Count();
            ViewBag.YorumSayisi = db.Yorums.Count();
            ViewBag.KategoriSayisi = db.Kategoris.Count();
            ViewBag.KullaniciSayisi = db.Uyes.Count();
            return View();
        }

    }
}