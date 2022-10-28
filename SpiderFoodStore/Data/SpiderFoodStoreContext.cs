using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SpiderFoodStore.Data
{
    public class SpiderFoodStoreContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SpiderFoodStoreContext() : base("name=SpiderFoodStoreContext")
        {
        }

        public System.Data.Entity.DbSet<SpiderFoodStore.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<SpiderFoodStore.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<SpiderFoodStore.Models.OrderInvoice> OrderInvoices { get; set; }

        public System.Data.Entity.DbSet<SpiderFoodStore.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<SpiderFoodStore.Models.SaleCode> SaleCodes { get; set; }

        public System.Data.Entity.DbSet<SpiderFoodStore.Models.Admin> Admins { get; set; }
    }
}
