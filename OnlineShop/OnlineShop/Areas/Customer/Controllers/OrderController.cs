using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Customer.Models;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        //Get Checkout Action Method
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }
        //Post Checkout Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            //get session
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    orderDetails.OrderStatus = "Pending";
                    orderDetails.UserId = HttpContext.Session.GetString("UserId");
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo = GetOrderNo();
            _db.order.Add(order);
            await _db.SaveChangesAsync();
            //Set Session null
            HttpContext.Session.Remove("products");
            HttpContext.Session.SetString("OrderId", order.Id.ToString());
            return Redirect("/Customer/Order/Payment");
        }
        public string GetOrderNo()
        {
            int rowCount = _db.order.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
        [HttpGet]
        public IActionResult OrderDetails()
        {
            var data = (from OD in _db.orderDetails
                        join O in _db.order on OD.OrderId equals O.Id
                        join P in _db.products on OD.ProductId equals P.Id
                        join Payment in _db.payments on O.Id equals Payment.OrderId into paymentGroup
                        from payment in paymentGroup.DefaultIfEmpty()  // Left join
                        where OD.UserId == HttpContext.Session.GetString("UserId")
                        select new OrdersDetails
                        {
                            Id = OD.Id,
                            //OrderId = O.Id,
                            //ProductId = P.Id,
                            //OrderNo = O.OrderNo,
                            //CustomerName = O.Name,
                            //PhoneNo = O.PhoneNo,
                            //Email = O.Email,
                            //Address = O.Address,
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
        public IActionResult Payment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Payment(Payment payment)
        {
            payment.OrderId = Convert.ToInt32(HttpContext.Session.GetString("OrderId"));
            payment.Date = DateTime.Now;
            _db.payments.Add(payment);
            _db.SaveChanges();
            HttpContext.Session.Remove("OrderId");
            return Redirect("/Customer/Home");
        }
    }
}
