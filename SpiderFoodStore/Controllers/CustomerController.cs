using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrypt;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;

namespace SpiderFoodStore.Controllers
{
    public class CustomerController : Controller
    {
        private SpiderFoodStoreContext db = new SpiderFoodStoreContext();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer customer)
        {
            Customer user = (from c in db.Customers where c.Username.Equals(customer.Username) select c).SingleOrDefault();
            if(user == null)
            {
                ViewBag.Error = "Username or password is invalid";
                return View();
            }

            ScryptEncoder encoder = new ScryptEncoder();
            bool isValidCustomer = encoder.Compare(customer.Password, user.Password);
            if (isValidCustomer)
            {
                Session["Customer"] = user;
                if (TempData["Route"] != null && TempData["Route"].ToString() == "2")
                {
                    return RedirectToAction("CheckOut", "ShoppingCart");
                }
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Error = "Username or password is invalid";
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer customer, string rePassword)
        {
            Customer user = (from c in db.Customers where c.Username.Equals(customer.Username) || c.Email.Equals(customer.Email) select c).SingleOrDefault();
            if(user != null)
            {
                if(user.Username == customer.Username)
                {
                    ViewBag.Error = "Username is exists";
                    return View();
                }
                if(user.Email == customer.Email)
                {
                    ViewBag.Error = "Email is exists";
                    return View();
                }
            }
            if(customer.Password != rePassword)
            {
                ViewBag.Error = "Password and repassword isn't match";
                return View();
            }
            ScryptEncoder encode = new ScryptEncoder();
            customer.Password = encode.Encode(customer.Password);
            db.Customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            Session["Customer"] = null;
            return RedirectToAction("Index", "Product");
        }
        public ActionResult Account()
        {
            if (Session["Customer"] != null)
            {
                ViewBag.Customer = Session["Customer"] as Customer;
            }
            return PartialView();
        }
        public new ActionResult Profile()
        {
            if (Session["Customer"] == null)
            {
                return RedirectToAction("ErrorProfile");
            }
            return View(Session["Customer"] as Customer);
        }
        [HttpPost]
        public ActionResult EditProfile(FormCollection frm, HttpPostedFileBase imageFile)
        {
            if (String.IsNullOrEmpty(frm["firstname"]))
            {
                ViewBag.Error = "Firstname isn't null";
                return RedirectToAction("Profile");
            } else
            if (String.IsNullOrEmpty(frm["lastname"]))
            {
                ViewBag.Error = "Lastname isn't null";
                return RedirectToAction("Profile");
            }
            else
            if (String.IsNullOrEmpty(frm["phone"]))
            {
                ViewBag.Error = "Phone isn't null";
                return RedirectToAction("Profile");
            }
            else
            if (String.IsNullOrEmpty(frm["address"]))
            {
                ViewBag.Error = "Address isn't null";
                return RedirectToAction("Profile");
            }
            int id = int.Parse(frm["id"].ToString());
            Customer customer = db.Customers.FirstOrDefault(n => n.Id == id);
            customer.Firstname = frm["firstname"].ToString();
            customer.Lastname = frm["lastname"].ToString();
            customer.Phone = frm["phone"].ToString();
            customer.Address = frm["address"].ToString();
            if (imageFile != null)
            {
                string fileName = Path.GetFileName(imageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                if (!System.IO.File.Exists(path))
                {
                    imageFile.SaveAs(path);
                }
                customer.ImagePath = fileName;
            }
            db.Customers.AddOrUpdate(customer);
            db.SaveChanges();
            Session["Customer"] = customer;
            return RedirectToAction("Profile");
        }
        public ActionResult ErrorProfile()
        {
            return View();
        }
    }
}