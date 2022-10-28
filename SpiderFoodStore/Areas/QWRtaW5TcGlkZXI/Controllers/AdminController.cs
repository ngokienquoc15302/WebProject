using Scrypt;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SpiderFoodStore.Areas.QWRtaW5TcGlkZXI.Controllers
{
    public class AdminController : Controller
    {
        SpiderFoodStoreContext db = new SpiderFoodStoreContext();
        // GET: QWRtaW5TcGlkZXI/Admin
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            string txtUsername = frm["username"].ToString();
            string txtPassword = frm["password"].ToString();
            ScryptEncoder encoding = new ScryptEncoder();
            Admin admin = db.Admins.FirstOrDefault(n => n.Username == txtUsername);
            if(admin != null && encoding.Compare(txtPassword, admin.Password))
            {
                Session["Admin"] = admin;
                return RedirectToAction("Index", "Products");
            }
            ViewBag.Message = "username or password isn't valid";
            return View();
        }
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login");
        }
    }
}