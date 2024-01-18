using OnlineShop.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnlineShop.Areas.Customer.Models
{
    public class OrdersDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string OrderNo { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }
        public bool IsAvailable { get; set; }
        public int ProductTypeId { get; set; }
        public ProductTypes ProductTypes { get; set; }
        public int SpecialTagId { get; set; }
        public SpecialTags SpecialTags { get; set; }
        [Display(Name = "Transaction Id")]
        public string TransactionId { get; set; }
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
    }
}
