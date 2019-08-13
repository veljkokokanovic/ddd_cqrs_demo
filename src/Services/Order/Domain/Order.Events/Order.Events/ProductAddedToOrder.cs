using System;
using domainD;

namespace Order.Events
{
    public class ProductAddedToOrder : DomainEvent
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Guid UserId { get; set; }
    }
}
