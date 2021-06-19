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

        public LocationController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAddressForAllPharmacies()
        {
            // TODO: remove filtering for pharmacies by street, but do it by user roles tables and references.
            var pharmacies = _userManager.Users
                .Where(x => !string.IsNullOrEmpty(x.Street))
                .Select(x => new { Name = x.Email, Address = $"{x.City} {x.Street}" })
                .ToList();

            return Json(pharmacies);
        }
    }
}
