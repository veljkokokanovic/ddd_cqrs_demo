using System;
using domainD;

namespace UI.Gateway.Models.Order.Commands
{
    public class RemoveProductViewModel
    {
        public string Sku { get; set; }

        public Guid OrderId { get; set; }
    }
}
