using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothesWeb.Models;
using Newtonsoft.Json;

namespace ClothesWeb.Controllers
{
    public class BillsController : Controller
    {
        private clothesEntities db = new clothesEntities();

        // GET: Bills
        public ActionResult Index()
        {
            var adminCookie = Request.Cookies["admin"];
            if(adminCookie == null)
            {
                return RedirectToAction("Index","Products");
            }
            var bill = db.Bill.Include(b => b.Customer).Include(b => b.User);
            return View(bill.ToList());
        }

        public ActionResult Chart()
        {
            var bills = from DT in db.DetailBIll
                        where
                          SqlFunctions.DatePart("month", DT.Bill.createdAt) == SqlFunctions.DatePart("month", SqlFunctions.GetDate())
                        group DT.Bill by new
                        {
                            Column1 = SqlFunctions.DateName("day", DT.Bill.createdAt) + "/" + SqlFunctions.DateName("month", DT.Bill.createdAt) + "/" + SqlFunctions.DateName("year", DT.Bill.createdAt)

                        } into g
                        orderby
                          g.Key.Column1
                        select new
                        {
                            Date = g.Key.Column1,
                            totalMoney = (double?)g.Sum(p => p.Total)
                        };
            var list = JsonConvert.SerializeObject(bills, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Content(list, "application/json");
        }

        // GET: Bills/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bill.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bills/Create
        public ActionResult Create()
        {
            ViewBag.idCustomer = new SelectList(db.Customer, "idCustomer", "name");
            ViewBag.idUser = new SelectList(db.User, "idUser", "idPermission");
            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idBill,idUser,idCustomer,Shipping,Total,PTTT,status,createdAt,TotalQty")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Bill.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCustomer = new SelectList(db.Customer, "idCustomer", "name", bill.idCustomer);
            ViewBag.idUser = new SelectList(db.User, "idUser", "idPermission", bill.idUser);
            return View(bill);
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bill.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCustomer = new SelectList(db.Customer, "idCustomer", "name", bill.idCustomer);
            ViewBag.idUser = new SelectList(db.User, "idUser", "idPermission", bill.idUser);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idBill,idUser,idCustomer,Shipping,Total,PTTT,status,createdAt,TotalQty")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCustomer = new SelectList(db.Customer, "idCustomer", "name", bill.idCustomer);
            ViewBag.idUser = new SelectList(db.User, "idUser", "idPermission", bill.idUser);
            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bill.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Bill bill = db.Bill.Find(id);
            db.Bill.Remove(bill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
