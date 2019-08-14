using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Order.Commands
{
    public class RemoveProduct : ICommand
    {
        public string Sku { get; set; }

        public Guid OrderId { get; set; }
    }
}
