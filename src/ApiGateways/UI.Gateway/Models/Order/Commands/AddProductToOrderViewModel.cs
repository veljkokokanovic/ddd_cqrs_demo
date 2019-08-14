using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Gateway.Models.Order.Commands
{
    public class AddProductToOrderViewModel
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public decimal Price { get; set; }
    }
}
