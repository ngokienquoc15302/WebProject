using SpiderFoodStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpiderFoodStore.Models;

namespace SpiderFoodStore.Models
{
    public class ShoppingCart
    {
        private SpiderFoodStoreContext db = new SpiderFoodStoreContext();
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal Money { get { return Amount * Price; } }

        public ShoppingCart(int Id)
        {
            Product product = db.Products.Where(n => n.Id == Id).FirstOrDefault();
            this.Id = Id;
            this.Name = product.Name;
            this.ImagePath = product.ImagePath;
            this.Price = product.Price;
            this.Amount = 1;
        }
    }
}
