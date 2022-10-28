using PagedList;
using SpiderFoodStore.Data;
using SpiderFoodStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpiderFoodStore.Controllers
{
    public class ProductController : Controller
    {
        SpiderFoodStoreContext db = new SpiderFoodStoreContext();
        // GET: Product
        public ActionResult Index()
        {
            ViewBag.FeaturnedProduct = db.Products.Select(n => n).Where(n => n.isHide == false).ToList();
            ViewBag.Categories = db.Categories.Select(n => n).ToList();
            ViewBag.LatestProduct = db.Products.Select(n => n).OrderByDescending(n => n.AtUpdate).Where(n => n.isHide == false).Take(6).ToList();
            ViewBag.MostBoughtProduct = db.Products.Select(n => n).OrderByDescending(n => n.QuanlityPurchased).Where(n => n.isHide == false).Take(6).ToList();
            ViewBag.MostViewedProduct = db.Products.Select(n => n).OrderByDescending(n => n.NumberOfViews).Where(n => n.isHide == false).Take(6).ToList();
            return View();
        }
        public ActionResult Menu()
        {
            List<Category> category = db.Categories.Select(n => n).ToList();
            return PartialView(category);
        }
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = db.Products.Where(n => n.Id == id).FirstOrDefault();
            if(product == null)
            {
                return HttpNotFound();
            }
            ViewBag.LatestProduct = db.Products.Select(n => n).OrderByDescending(n => n.AtUpdate).Where(n => n.isHide == false).Take(4).ToList();
            return View(product);
        }
        public ActionResult ShopGrid(int? page, string searchString, int? category)
        {
            ViewBag.LatestProduct = db.Products.Select(n => n).Where(n => n.isHide == false).OrderByDescending(n => n.AtUpdate).Take(6).ToList();
            ViewBag.Categories = db.Categories.ToList();
            int iPageNum = (page ?? 1);
            int iPageSize = 9;
            List<Product> list = db.Products.Where(n => n.isHide == false).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(n => n.Name.Contains(searchString)).ToList();
            }
            if(category != null)
            {
                list = list.Where(n => n.CategoryId.Equals((int)category)).ToList();
            }
            ViewBag.Sum = list.Count();
            return View(list.ToPagedList(iPageNum, iPageSize));
        }
    }
}