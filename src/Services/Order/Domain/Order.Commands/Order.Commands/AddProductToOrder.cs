using System;
using domainD;

namespace Order.Commands
{
    public class AddProductToOrder : ICommand
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }
    }
}
