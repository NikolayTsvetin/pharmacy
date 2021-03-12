using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Product name must not be longer than 100 characaters.")]
        public string Name { get; set; }

        [Required]
        [Range(0.1, 10000)]
        public decimal Price { get; set; }

        public IFormFile Photo { get; set; }
    }
}
