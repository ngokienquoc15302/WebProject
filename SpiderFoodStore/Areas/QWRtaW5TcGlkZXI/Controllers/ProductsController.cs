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
using System.Web.UI.WebControls;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;

namespace SpiderFoodStore.Areas.QWRtaW5TcGlkZXI.Controllers
{
    public class ProductsController : Controller
    {
        private SpiderFoodStoreContext db = new SpiderFoodStoreContext();

        // GET: QWRtaW5TcGlkZXI/Products
        public ActionResult Index()
        {
            return View(db.Products.OrderByDescending(n => n.AtUpdate).ToList());
        }

        // GET: QWRtaW5TcGlkZXI/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryTitle = db.Categories.Find(product.CategoryId).Name;
            return View(product);
        }

        // GET: QWRtaW5TcGlkZXI/Products/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList().OrderBy(n => n.Id), "Id", "Name");
            return View();
        }

        // POST: QWRtaW5TcGlkZXI/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Size,Weight,Description,ImagePath,ImageFile,RemainingAmount,isHide")] Product product, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                if(product.ImageFile != null)
                {
                    string fileName = Path.GetFileName(product.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (!System.IO.File.Exists(path))
                    {
                        product.ImageFile.SaveAs(path);
                    }
                    product.ImagePath = fileName;
                }
                product.AtUpdate = DateTime.Now;
                product.QuanlityPurchased = 0;
                product.NumberOfViews = 0;
                product.CategoryId = int.Parse(frm["Categories"]);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: QWRtaW5TcGlkZXI/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = new SelectList(db.Categories.ToList().OrderBy(n => n.Id), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: QWRtaW5TcGlkZXI/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Size,Weight,Description,ImagePath,ImageFile,RemainingAmount,QuanlityPurchased,NumberOfViews,isHide")] Product product, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                product.AtUpdate = DateTime.Now;
                if(product.ImageFile != null)
                {
                    string fileName = Path.GetFileName(product.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (!System.IO.File.Exists(path))
                    {
                        product.ImageFile.SaveAs(path);
                    }
                    product.ImagePath = fileName;
                }
                else
                {
                    product.ImagePath = db.Products.SingleOrDefault(n => n.Id == product.Id).ImagePath;
                }
                product.CategoryId = int.Parse(frm["Categories"]);
                db.Products.AddOrUpdate(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: QWRtaW5TcGlkZXI/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: QWRtaW5TcGlkZXI/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
