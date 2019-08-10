using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Gateway.Models.Order.Commands
{
    public class SetProductQuantity
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }
    }
}
