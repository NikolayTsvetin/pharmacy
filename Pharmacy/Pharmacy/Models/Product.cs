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
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
