using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }

        public string Street { get; set; }

        public List<Product> Products { get; set; }
    }
}
