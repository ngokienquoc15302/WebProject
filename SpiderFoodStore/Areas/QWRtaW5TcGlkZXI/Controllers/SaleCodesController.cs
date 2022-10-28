using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;

namespace SpiderFoodStore.Areas.QWRtaW5TcGlkZXI.Controllers
{
    public class SaleCodesController : Controller
    {
        private SpiderFoodStoreContext db = new SpiderFoodStoreContext();

        // GET: QWRtaW5TcGlkZXI/SaleCodes
        public ActionResult Index()
        {
            return View(db.SaleCodes.ToList());
        }

        // GET: QWRtaW5TcGlkZXI/SaleCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleCode saleCode = db.SaleCodes.Find(id);
            if (saleCode == null)
            {
                return HttpNotFound();
            }
            return View(saleCode);
        }

        // GET: QWRtaW5TcGlkZXI/SaleCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QWRtaW5TcGlkZXI/SaleCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code,DiscountValue")] SaleCode saleCode)
        {
            if (ModelState.IsValid)
            {
                db.SaleCodes.Add(saleCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saleCode);
        }

        // GET: QWRtaW5TcGlkZXI/SaleCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleCode saleCode = db.SaleCodes.Find(id);
            if (saleCode == null)
            {
                return HttpNotFound();
            }
            return View(saleCode);
        }

        // POST: QWRtaW5TcGlkZXI/SaleCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,DiscountValue")] SaleCode saleCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saleCode);
        }

        // GET: QWRtaW5TcGlkZXI/SaleCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleCode saleCode = db.SaleCodes.Find(id);
            if (saleCode == null)
            {
                return HttpNotFound();
            }
            return View(saleCode);
        }

        // POST: QWRtaW5TcGlkZXI/SaleCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleCode saleCode = db.SaleCodes.Find(id);
            db.SaleCodes.Remove(saleCode);
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
