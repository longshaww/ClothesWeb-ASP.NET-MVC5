using ClothesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesWeb.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            Cart cart = Session["Cart"] as Cart;
            if (Session["Cart"] == null || cart.CartCount() < 1)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(cart);
        }
    }
}