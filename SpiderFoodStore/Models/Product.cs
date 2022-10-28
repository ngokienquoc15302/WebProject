using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SpiderFoodStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string Size { get; set; }
        public string Weight { get; set; }
        public string Description { get; set; }
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        public DateTime AtUpdate { get; set; }
        [DisplayName("Remaining amount")]
        public int RemainingAmount { get; set; }
        [DisplayName("Quanlity purchased")]
        public int QuanlityPurchased { get; set; }
        [DisplayName("Number of views")]
        public int NumberOfViews { get; set; }
        [DisplayName("Status")]
        public bool isHide { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}