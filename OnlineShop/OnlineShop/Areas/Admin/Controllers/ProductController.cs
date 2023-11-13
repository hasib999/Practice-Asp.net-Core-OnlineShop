using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_db.products.Include(c=>c.ProductTypes).Include(f=>f.SpecialTags).ToList());
        }
        //Post Index Action Method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.products.Include(c => c.ProductTypes).Include(f => f.SpecialTags)
                .Where(c=>c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                products = _db.products.Include(c => c.ProductTypes).Include(f => f.SpecialTags).ToList();
            }
            return View(products);
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
                var searchProduct = _db.products.FirstOrDefault(c => c.Name == products.Name);
                if(searchProduct != null)
                {
                    ViewBag.message = "This Product is already exist";
                    ViewData["ProductTypeId"] = new SelectList(_db.productTypes.ToList(), "Id", "productType");
                    ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "ProductTag");
                    return View(products);
                }
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
                TempData["save"] = "Product Saved Successfully";
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
        //Post Edit Method
        [HttpPost]
        public async Task<IActionResult> Edit(Products products,IFormFile image)
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
                _db.products.Update(products);
                await _db.SaveChangesAsync();
                TempData["update"] = "Product Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Get Details Action Method
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTags).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Get Delete Action Method
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTags).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Post Delete Action Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["delete"] = "Product Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
