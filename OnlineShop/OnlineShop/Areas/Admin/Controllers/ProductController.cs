using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.products.Include(c=>c.ProductTypes).Include(f=>f.SpecialTags).ToList());
        }

        //Get Create Method
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(),"Id","productType"); 
            ViewData["TagId"] = new SelectList(_db.specialTags.ToList(),"Id", "ProductTag"); 
            return View();
        }
        //Post Create Method
        [HttpPost]
        public async Task<IActionResult> Create(Products products,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath+"/Images",Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/"+image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noimage.png";
                }
                _db.products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Get Edit Method
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(), "Id", "productType");
            ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "ProductTag");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(f => f.SpecialTags).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
