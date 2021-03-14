using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.ViewModels
{
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public Guid Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
