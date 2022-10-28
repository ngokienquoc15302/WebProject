using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scrypt;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;

namespace SpiderFoodStore.Areas.QWRtaW5TcGlkZXI.Controllers
{
    public class CustomersController : Controller
    {
        private SpiderFoodStoreContext db = new SpiderFoodStoreContext();

        // GET: QWRtaW5TcGlkZXI/Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: QWRtaW5TcGlkZXI/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: QWRtaW5TcGlkZXI/Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QWRtaW5TcGlkZXI/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Firstname,Lastname,Username,Password,ImagePath,ImageFile,Email,Phone,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                ScryptEncoder encoder = new ScryptEncoder();
                customer.Password = encoder.Encode(customer.Password);
                if(customer.ImageFile != null)
                {
                    string fileName = Path.GetFileName(customer.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (!System.IO.File.Exists(path))
                    {
                        customer.ImageFile.SaveAs(path);
                    }
                }
                customer.ImagePath = customer.ImageFile != null ? Path.GetFileName(customer.ImageFile.FileName) : "avatar-default.png";
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: QWRtaW5TcGlkZXI/Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            customer.Password = "default";
            return View(customer);
        }

        // POST: QWRtaW5TcGlkZXI/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,Password,Username,ImagePath,ImageFile,Email,Phone,Address")] Customer customer, string iPassword)
        {
            if (ModelState.IsValid)
            {
                Customer oldCustomer = db.Customers.FirstOrDefault(n => n.Id == customer.Id);
                if (iPassword != "")
                {
                    ScryptEncoder encoder = new ScryptEncoder();
                    customer.Password = encoder.Encode(iPassword);
                }
                else
                {
                    customer.Password = oldCustomer.Password;
                }
                if(customer.ImageFile != null)
                {
                    string fileName = Path.GetFileName(customer.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (!System.IO.File.Exists(path))
                    {
                        customer.ImageFile.SaveAs(path);
                    }
                    customer.ImagePath = fileName;
                }
                else
                {
                    customer.ImagePath = oldCustomer.ImagePath;
                }
                db.Customers.AddOrUpdate(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: QWRtaW5TcGlkZXI/Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: QWRtaW5TcGlkZXI/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
