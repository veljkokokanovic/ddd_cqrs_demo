using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Api.Models
{
    public class ProductViewModel
    {
        public ProductViewModel(string sku, string name, string category)
        {
            Sku = sku;
            Name = name;
            Category = category;
        }

        public string Sku { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}
