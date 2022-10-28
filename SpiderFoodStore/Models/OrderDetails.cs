using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpiderFoodStore.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderInvoiceId { get; set; }
        [Key]
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Money { get { return Price * Amount; } }
    }
}