using System;
using domainD;

namespace Order.Events
{
    public class ProductAddedToOrder : DomainEvent
    {
        public ProductAddedToOrder(string sku, int quantity, decimal price, Guid userId)
        {
            Sku = sku;
            Quantity = quantity;
            Price = price;
            UserId = userId;
        }

        public string Sku { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Guid UserId { get; set; }
    }
}
