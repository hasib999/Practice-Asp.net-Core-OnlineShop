using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if(products != null)
            {
                foreach(var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo = GetOrderNo();
            _db.order.Add(order);
            await _db.SaveChangesAsync();
            //Set Session null
            HttpContext.Session.Remove("products"); 
            return Redirect("/Customer/Home");
        }
        public string GetOrderNo()
        {
            int rowCount = _db.order.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}
