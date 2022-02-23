using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ApplicationUser:IdentityUser //inherit identity user
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
