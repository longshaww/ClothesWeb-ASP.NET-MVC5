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
        private clothesEntities db = new clothesEntities();
        // GET: Payment
        public ActionResult Index(String error)
        {
            Cart cart = Session["Cart"] as Cart;
            if (Session["Cart"] == null || cart.CartCount() < 1)
            {
                return RedirectToAction("Index", "Products");
            }
            if(error != null)
            {
                ViewBag.error = error;
            }
            return View(cart);
        }


        public ActionResult SuccessPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(FormCollection form)
        {
            try { 
            Cart cart = Session["Cart"] as Cart;
            if (form["cusName"] == "" || form["cusPhone"] == "" || form["cusAddress"] == "" || form["cusEmail"] == "")
            {
                return (RedirectToAction("Index", new { error = "Vui lòng nhập đầy đủ thông tin" }));
            }

            Guid cusId = Guid.NewGuid();
            Customer customer = new Customer();
            customer.idCustomer = cusId.ToString();
            customer.name = form["cusName"];
            customer.phone = form["cusPhone"];
            customer.address = form["cusAddress"];
            customer.email = form["cusEmail"];
            db.Customer.Add(customer);

            Guid billId = Guid.NewGuid();
            Bill bill = new Bill();
            bill.idBill = billId.ToString();
            bill.idCustomer = customer.idCustomer;
            bill.PTTT = form["PaymentMethod"];
            bill.Shipping = 15;
            bill.status = false;
            bill.Total = cart.TotalMoney();
            bill.TotalQty = cart.CartCount();
            bill.createdAt = DateTime.Now;
            db.Bill.Add(bill);

            foreach(var item in cart.Items)
            {
                Guid detailBillId = Guid.NewGuid();
                DetailBIll detailBill = new DetailBIll();
                detailBill.idDetailBill = detailBillId.ToString();
                detailBill.idBill = bill.idBill;
                detailBill.idProduct = item._shopping_product.idProduct;
                detailBill.qty = item._shopping_qty;
                detailBill.ProductTotal = item._shopping_product.price * item._shopping_qty;
                detailBill.Size = item._shopping_size;
                db.DetailBIll.Add(detailBill);
            }
            db.SaveChanges();
            cart.ClearCart();
                return RedirectToAction("SuccessPayment", "Payment");
            }
            catch
            {
                return Content("Error Checkout");
            }
        }
    }   
}