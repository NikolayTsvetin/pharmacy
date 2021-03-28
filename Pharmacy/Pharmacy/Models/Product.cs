using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Product name must not be longer than 100 characaters.")]
        public string Name { get; set; }

        [Required]
        [Range(0.1, 10000)]
        public decimal Price { get; set; }

        public string PhotoPath { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
