using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpiderFoodStore.Models
{
    public class SaleCode
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [DisplayName("Discount Value")]
        public decimal DiscountValue { get; set; }
    }
}