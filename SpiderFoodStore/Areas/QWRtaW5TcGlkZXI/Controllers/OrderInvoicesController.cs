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
    public class OrderInvoicesController : Controller
    {
        private SpiderFoodStoreContext db = new SpiderFoodStoreContext();

        // GET: QWRtaW5TcGlkZXI/OrderInvoices
        public ActionResult Index()
        {
            return View(db.OrderInvoices.OrderByDescending(n => n.OrderDate).ToList());
        }

        // GET: QWRtaW5TcGlkZXI/OrderInvoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderInvoice orderInvoice = db.OrderInvoices.Find(id);
            if (orderInvoice == null)
            {
                return HttpNotFound();
            }
            return View(orderInvoice);
        }

        // GET: QWRtaW5TcGlkZXI/OrderInvoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QWRtaW5TcGlkZXI/OrderInvoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,OrderDate,Total,DeliveryDate,NameOfConsignee,AddressOfConsignee,PhoneOfConsignee,isDelivered,isPaid")] OrderInvoice orderInvoice)
        {
            if (ModelState.IsValid)
            {
                db.OrderInvoices.Add(orderInvoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderInvoice);
        }

        // GET: QWRtaW5TcGlkZXI/OrderInvoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderInvoice orderInvoice = db.OrderInvoices.Find(id);
            if (orderInvoice == null)
            {
                return HttpNotFound();
            }
            return View(orderInvoice);
        }

        // POST: QWRtaW5TcGlkZXI/OrderInvoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,OrderDate,Total,DeliveryDate,NameOfConsignee,AddressOfConsignee,PhoneOfConsignee,isDelivered,isPaid")] OrderInvoice orderInvoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderInvoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderInvoice);
        }

        // GET: QWRtaW5TcGlkZXI/OrderInvoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderInvoice orderInvoice = db.OrderInvoices.Find(id);
            if (orderInvoice == null)
            {
                return HttpNotFound();
            }
            return View(orderInvoice);
        }

        // POST: QWRtaW5TcGlkZXI/OrderInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderInvoice orderInvoice = db.OrderInvoices.Find(id);
            db.OrderInvoices.Remove(orderInvoice);
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
