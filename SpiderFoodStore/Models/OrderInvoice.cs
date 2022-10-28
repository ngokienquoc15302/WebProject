using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpiderFoodStore.Models
{
    public class OrderInvoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OrderId { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [DisplayName("Order date")]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        [DisplayName("Delivery date")]
        public DateTime DeliveryDate { get; set; }
        [DisplayName("Name of consignee")]
        public string NameOfConsignee { get; set; }
        [DisplayName("Address of consignee")]
        public string AddressOfConsignee { get; set; }
        [DisplayName("Phone of consignee")]
        public string PhoneOfConsignee { get; set; }
        [Required]
        [DisplayName("Delivered")]
        public bool isDelivered { get; set; }
        [Required]
        [DisplayName("Paid")]
        public bool isPaid { get; set; }
    }
}