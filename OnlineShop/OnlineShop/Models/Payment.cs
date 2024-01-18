using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [Required]
        [Display(Name = "Transaction Id")]
        public string TransactionId { get; set; }
        public DateTime Date {  get; set; }
    }
}
