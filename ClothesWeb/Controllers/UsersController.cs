using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothesWeb.Models;

namespace ClothesWeb.Controllers
{
    public class UsersController : Controller
    {
        private clothesEntities db = new clothesEntities();

        // GET: Users
        public ActionResult Index()
        {
            var adminCookie = Request.Cookies["admin"];
            var userCookie = Request.Cookies["user"];
            if (adminCookie == null && userCookie !=null) {
                var user = db.User.Where(s => s.username == userCookie.Value).FirstOrDefault();
                var bills = db.Bill.Where(s => s.idUser.Equals(user.idUser)).Include(s=>s.DetailBIll).ToList();
                ViewBag.bills = bills;
                ViewBag.user = user;
            }
            if (adminCookie != null && userCookie != null)
            {
                var userAdmin = db.User.Where(s => s.username == adminCookie.Value).FirstOrDefault();
                ViewBag.user = userAdmin;
            }
            var users = db.User.Include(u => u.Permission);
            return View(users.ToList());
        }



        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.idPermission = new SelectList(db.Permission, "idPermission", "namePermission");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idUser,idPermission,username,password,gender,identityCard,address,email,phone,name")] User user, HttpPostedFileBase avatar)
        {
            try
            {
                if (avatar != null)
                {
                    var InputFileName = Path.GetFileName(avatar.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Content/images/") + InputFileName);
                    avatar.SaveAs(ServerSavePath);
                    user.URLAvatar = "~/Content/images/" + avatar.FileName;
                }

                if (user.idPermission == null)
                {
                    user.idPermission = "P2";
                }
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idPermission = new SelectList(db.Permission, "idPermission", "namePermission", user.idPermission);
                if (Request.Cookies["admin"] != null)
                {
                    return View(user);
                }
                return Redirect("/login");
            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPermission = new SelectList(db.Permission, "idPermission", "namePermission", user.idPermission);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idUser,idPermission,username,password,gender,identityCard,address,email,URLAvatar,phone,name")] User user, HttpPostedFileBase avatar)
        {
            try
            {

            if (avatar != null)
            {
                var InputFileName = Path.GetFileName(avatar.FileName);
                var ServerSavePath = Path.Combine(Server.MapPath("~/Content/images/") + InputFileName);
                avatar.SaveAs(ServerSavePath);
                user.URLAvatar = "~/Content/images/" + avatar.FileName;
            }
            if (user.idPermission == null)
            {
                user.idPermission = "P2";
            }
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idPermission = new SelectList(db.Permission, "idPermission", "namePermission", user.idPermission);
                if (Request.Cookies["admin"] != null)
                {
                    return View(user);
                }
                return Redirect("/login");
            
            }catch(Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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
