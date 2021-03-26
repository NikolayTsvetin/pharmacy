using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public SearchController(PharmacyContext pharmacyContext)
        {
            _pharmacyContext = pharmacyContext;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchInput, string city)
        {
            // TODO: implement filter by city.
            List<Product> products = await _pharmacyContext.Products.ToListAsync();
            List<Product> searchedProducts = products
                .Where(x => x.Name.ToLower().IndexOf(searchInput.ToLower()) >= 0)
                .ToList();

            return View();
        }
    }
}
