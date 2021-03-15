using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using Pharmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PharmacyContext _pharmacyContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductsController(PharmacyContext pharmacyContext, IHostingEnvironment hostingEnvironment)
        {
            _pharmacyContext = pharmacyContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Price = model.Price,
                    PhotoPath = UploadPhotoAndReturnPhotoPath(model.Photo)
                };

                if (string.IsNullOrEmpty(product.PhotoPath))
                {
                    product.PhotoPath = @"no-image.svg";
                }

                await _pharmacyContext.AddAsync(product);
                await _pharmacyContext.SaveChangesAsync();

                return Redirect("AllProducts");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllProducts()
        {
            List<Product> allProducts = await _pharmacyContext.Products.ToListAsync();

            return View(allProducts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (Guid.TryParse(id, out Guid idAsGuid))
            {
                Product product = await _pharmacyContext.Products.FindAsync(idAsGuid);

                if (product == null)
                {
                    // TODO: Fix with NotFound view.
                    return View("Error");
                }

                ProductEditViewModel model = new ProductEditViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ExistingPhotoPath = product.PhotoPath
                };

                return View(model);
            }

            // TODO: Fix with NotFound view.
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product foundProduct = await _pharmacyContext.Products.FindAsync(model.Id);
                foundProduct.Name = model.Name;
                foundProduct.Price = model.Price;

                // Before adding new photo, delete the current one.
                if (!string.IsNullOrEmpty(foundProduct.PhotoPath) && model.Photo != null)
                {
                    string pathToExistingPhoto = Path.Combine(_hostingEnvironment.WebRootPath, "images", foundProduct.PhotoPath);

                    System.IO.File.Delete(pathToExistingPhoto);
                    foundProduct.PhotoPath = UploadPhotoAndReturnPhotoPath(model.Photo);
                }

                _pharmacyContext.Products.Update(foundProduct);
                await _pharmacyContext.SaveChangesAsync();

                return Redirect("/Products/AllProducts");
            }

            return View(model);
        }

        private string UploadPhotoAndReturnPhotoPath(IFormFile photo)
        {
            string uniqueFileName = string.Empty;

            if (photo == null)
            {
                return uniqueFileName;
            }

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            uniqueFileName = $"{Guid.NewGuid()}_{photo.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fs);
            }

            return uniqueFileName;
        }
    }
}
