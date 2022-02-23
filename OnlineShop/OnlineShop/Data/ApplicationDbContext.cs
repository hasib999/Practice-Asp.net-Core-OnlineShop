using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProductTypes> productTypes { get; set; }
        public DbSet<SpecialTags> specialTags { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
    }
}
