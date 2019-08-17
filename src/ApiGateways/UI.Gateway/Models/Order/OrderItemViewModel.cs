using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Gateway.Models.Order
{
    public class OrderItemViewModel
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Sku { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}
