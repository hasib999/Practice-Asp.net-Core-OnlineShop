using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace OnlineShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {
            return View(_db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTags).ToList().ToPagedList(page ?? 1, 9));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Get Product Details Action Method
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id == null){
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Post Product Details Action Method
        [HttpPost]
        [ActionName("Details")]
        public ActionResult ProductDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if(id == null){
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            //get list of product everytime from session
            products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            //add single product on session
            HttpContext.Session.Set("products",products);
            return View(product);
        }
        //Get Remove Action Method
        [HttpGet]
        [ActionName("Remove")]
        [Authorize(Roles = "User")]
        public IActionResult RemoveToCart(int? id)
        {
            //get session
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    //remove product from session
                    products.Remove(product);
                    //session update
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Cart));
        }
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            //get session
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    //remove product from session
                    products.Remove(product);
                    //session update
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //Get Product cart action method
        [Authorize(Roles = "User")]
        public IActionResult Cart()
        {
            //get session
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }
    }
}
