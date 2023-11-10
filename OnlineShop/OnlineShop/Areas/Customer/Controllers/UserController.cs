using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _db;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.applicationUsers.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                
                applicationUser.EmailConfirmed = true;
                applicationUser.Email = applicationUser.UserName;
                var result = await _userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
                var isSaveRole = await _userManager.AddToRoleAsync(applicationUser, "User");
                if (result.Succeeded)
                {
                    TempData["save"] = "User Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                //validation
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser applicationUser)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == applicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["update"] = "User Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Lockout(ApplicationUser applicationUser)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == applicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.LockoutEnd = DateTime.Now.AddYears(100);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User Lockout Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Active(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Active(ApplicationUser applicationUser)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == applicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User Active Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Delete(ApplicationUser applicationUser)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == applicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            _db.applicationUsers.Remove(user);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["Delete"] = "User Delete Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
