using ClothesWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ClothesWeb.Models.User;

namespace ClothesWeb.Controllers
{
    public class LoginController : Controller
    {
        private clothesEntities db = new clothesEntities();
        // GET: Login
        public ActionResult Index(String errorUsername,String errorPassword)
        {
            if (Request.Cookies["user"] != null)
            {
                return RedirectToAction("Index", "Users");

            }
             RedirectToAction("Index", "Products");

            if (errorUsername != null )
                {
                    ViewBag.error = errorUsername;
                }
                if(errorPassword != null)
                {
                    ViewBag.error = errorPassword;
                }
                return View();
        }


        [HttpPost]
        public ActionResult Auth(FormCollection form)
        {

            String usernameForm = form["username"];
            String passwordForm = form["password"];

            var user = db.User.Where(s => s.username == usernameForm).FirstOrDefault();
            if (user == null)
            {
                return (RedirectToAction("Index", new { errorUsername = "Wrong username" }));
            }

            if (user.password != passwordForm)
            {
                return (RedirectToAction("Index", new { errorPassword = "Wrong password" }));
            }
            HttpCookie userCookie = new HttpCookie("user");
            userCookie.Value = usernameForm;
            userCookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(userCookie);

            var getPermssion = db.Permission.Where(s=>s.namePermission == "Admin").FirstOrDefault();

            if (user.idPermission == getPermssion.idPermission)
            {
                HttpCookie adminCookie = new HttpCookie("admin");
                adminCookie.Value = usernameForm;
                adminCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(adminCookie);
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Products");
        }



        public ActionResult Signout()
        {
            if (Request.Cookies["user"] != null)
            {
                HttpCookie user = Request.Cookies["user"];

                user.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(user);
            }

            if(Request.Cookies["admin"] != null)
            {
                HttpCookie admin = Request.Cookies["admin"];

                admin.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(admin);
            }

            return Redirect("/Products");
        }

    }
}