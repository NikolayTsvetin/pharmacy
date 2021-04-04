using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    public class SearchController : Controller
    {
        private readonly PharmacyContext _pharmacyContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController(PharmacyContext pharmacyContext, UserManager<ApplicationUser> userManager)
        {
            _pharmacyContext = pharmacyContext;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchInput, string city)
        {
            List<Product> products = await _pharmacyContext.Products.ToListAsync();
            IEnumerable<Product> searchedProducts = products.Where(x => x.Name.ToLower().IndexOf(searchInput.ToLower()) >= 0);

            if (string.IsNullOrEmpty(city))
            {
                TempData["products"] = JsonConvert.SerializeObject(searchedProducts.ToList());
                return RedirectToAction("Index", "Home");
            }

            List<Product> filteredProducts = new List<Product>();

            foreach (var product in searchedProducts)
            {
                ApplicationUser pharmacy = await _userManager.FindByIdAsync(product.ApplicationUserId);

                if (pharmacy.City.ToLower() == city.ToLower())
                {
                    filteredProducts.Add(product);
                }
            }

            TempData["products"] = JsonConvert.SerializeObject(filteredProducts);
            return RedirectToAction("Index", "Home");
        }
    }
}
