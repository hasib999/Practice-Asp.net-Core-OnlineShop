using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Customer.Models;
using OnlineShop.Data;
using System.Linq;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult OrderDetails()
        {
            var data = (from OD in _db.orderDetails
                        join O in _db.order on OD.OrderId equals O.Id
                        join P in _db.products on OD.ProductId equals P.Id
                        join Payment in _db.payments on O.Id equals Payment.OrderId into paymentGroup
                        from payment in paymentGroup.DefaultIfEmpty()  // Left join
                        select new OrdersDetails
                        {
                            Id = OD.Id,
                            //OrderId = O.Id,
                            //ProductId = P.Id,
                            OrderNo = O.OrderNo,
                            CustomerName = O.Name,
                            PhoneNo = O.PhoneNo,
                            Email = O.Email,
                            Address = O.Address,
                            OrderDate = O.OrderDate,
                            ProductName = P.Name,
                            Price = P.Price,
                            Image = P.Image,
                            ProductColor = P.ProductColor,
                            TransactionId = payment != null ? payment.TransactionId : null,
                            OrderStatus = OD.OrderStatus
                        }).OrderByDescending(x => x.Id).ToList();

            return View(data);
        }
        [HttpGet]
        public IActionResult Confirmed(OrdersDetails ordersDetails) 
        {
            var getData = _db.orderDetails.FirstOrDefault(x => x.Id == ordersDetails.Id);
            getData.OrderStatus = "Confirmed";
            _db.orderDetails.Update(getData);
            _db.SaveChanges();
            return Redirect("/Admin/Order/OrderDetails");
        }
    }
}
