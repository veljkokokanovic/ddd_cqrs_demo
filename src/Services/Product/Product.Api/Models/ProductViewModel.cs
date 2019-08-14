using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Api.Models
{
    public class ProductViewModel
    {
        public ProductViewModel(string sku, string name, string category, decimal price)
        {
            Sku = sku;
            Name = name;
            Category = category;
            Price = price;
        }

        public string Sku { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }
    }
}
