using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothesWeb.Models;

namespace ClothesWeb.Controllers
{

    public class ProductsController : Controller
    {
        private clothesEntities db = new clothesEntities();

        // GET: Products
        public ActionResult Index(String nameProduct, String idCollection,String isNew)
        {
            if (nameProduct != null)
            {
                return View(db.Product.Where(s => s.nameProduct.Contains(nameProduct)).Include(s => s.ImageProduct).ToList());
            }
            if (idCollection != null)
            {
                return View(db.Product.Where(s => s.idCollection.Equals(idCollection)).Include(s => s.ImageProduct).ToList());
            }
            if(isNew != null && isNew == "true")
            {
                return View(db.Product.Where(s => s.isNew).Include(s => s.ImageProduct).ToList());
            }
            var products = db.Product.Include(p => p.ImageProduct);
            return View(products.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Include(s => s.DetailBIll).Where(s => s.idProduct.Equals(id)).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.idCollection = new SelectList(db.Collection, "idCollection", "nameCollection");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nameProduct,idCollection,idProduct,sizeM,sizeL,sizeXL,price,isNew")] Product product, IEnumerable<HttpPostedFileBase> ImageUpload)
        {
            try
            {
                if (ModelState.IsValid && ImageUpload.Count() > 0)
                {
                    foreach (var file in ImageUpload)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Content/images/") + InputFileName);
                        file.SaveAs(ServerSavePath);
                        ImageProduct imgProd = new ImageProduct();
                        imgProd.idProduct = product.idProduct;
                        imgProd.idImage = Guid.NewGuid().ToString();
                        imgProd.URLImage = "~/Content/images/" + file.FileName;
                        db.ImageProduct.Add(imgProd);
                    }
                    product.isNew = true;
                    db.Product.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.idCollection = new SelectList(db.Collection, "idCollection", "nameCollection", product.idCollection);
                return View(product);
            }
            catch
            {
                return Content("Error");
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCollection = new SelectList(db.Collection, "idCollection", "nameCollection", product.idCollection);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nameProduct,idCollection,idProduct,sizeM,sizeL,sizeXL,price,isNew")] Product product, IEnumerable<HttpPostedFileBase> ImageUpload)
        {
            try
            {
                var delImages = db.ImageProduct.Where(s=>s.idProduct.Equals(product.idProduct));
                if (delImages != null)
                {
                    foreach(var image in delImages)
                    {
                        db.ImageProduct.Remove(image);
                    }
                }
                db.SaveChanges();
                if (ModelState.IsValid && ImageUpload.Count() > 0)
                {
                    foreach (var file in ImageUpload)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Content/images/") + InputFileName);
                        file.SaveAs(ServerSavePath);
                        ImageProduct imgProd = new ImageProduct();
                        imgProd.idProduct = product.idProduct;
                        imgProd.idImage = Guid.NewGuid().ToString();
                        imgProd.URLImage = "~/Content/images/" + file.FileName;
                        db.ImageProduct.Add(imgProd);
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idCollection = new SelectList(db.Collection, "idCollection", "nameCollection", product.idCollection);
                return View(product);
            }
            catch
            {
                return Content("Error");
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try { 
            var delImages = db.ImageProduct.Where(s => s.idProduct.Equals(id));
            if (delImages != null)
            {
                foreach (var image in delImages)
                {
                    db.ImageProduct.Remove(image);
                }
            }
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error");
            }
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
