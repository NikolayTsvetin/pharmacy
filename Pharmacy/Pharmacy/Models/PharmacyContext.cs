using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class PharmacyContext : IdentityDbContext<ApplicationUser>
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
