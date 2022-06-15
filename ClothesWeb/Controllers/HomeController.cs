using ClothesWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesWeb.Controllers
{
    public class HomeController : Controller
    {
        private clothesEntities db = new clothesEntities();

        public ActionResult Index()
        {
            return View(db.Product.Where(s => s.isNew).Include(s => s.ImageProduct).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}