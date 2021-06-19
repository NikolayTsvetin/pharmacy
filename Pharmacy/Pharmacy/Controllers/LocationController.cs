using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    public class LocationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LocationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressForAllPharmacies()
        {
            var pharmacies = await _userManager.GetUsersInRoleAsync("Pharmacy");
            var mapped = pharmacies.Select(x => new { Name = x.Email, Address = $"{x.City} {x.Street}" }).ToList();

            return Json(mapped);
        }
    }
}
