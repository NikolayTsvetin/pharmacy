using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class Pharmacy
    {
        public string City { get; set; }
        public List<Product> Products { get; set; }
    }
}
