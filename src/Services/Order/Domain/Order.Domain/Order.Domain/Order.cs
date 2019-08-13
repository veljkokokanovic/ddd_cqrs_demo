using System;
using System.Collections.Generic;
using System.Linq;
using domainD;
using Order.Events;

namespace Order.Domain
{
    public class Order : AggregateRoot
    {
        protected Order(Guid id) : base(id)
        {
        }

        private HashSet<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        private Address DeliveryAddress { get; set; }

        private string DeliveryDate { get; set; }

        private string PhoneNumber { get; set; }

        private Guid UserId { get; set; }

        public void AddProduct(string sku, int quantity, decimal price)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentNullException(nameof(sku));
            }

            if (quantity < 1)
            {
                throw new ArgumentException("Quantity must be > 0", nameof(quantity));
            }

            if (TryGetOrderItem(sku, out var item))
            {
                RaiseEvent(new ProductQuantityChanged { LineItemId = sku, From = item.Quantity, To = item.Quantity + quantity });
            }
            else
            {
                RaiseEvent(new ProductAddedToOrder { Quantity = quantity, Sku = sku, UserId = UserId, Price = price});
            }
        }

        private bool TryGetOrderItem(string sku, out OrderItem item)
        {
            item = Items.SingleOrDefault(i => string.Equals(i.Identity, sku));
            return item != null;
        }

        private void Handle(ProductAddedToOrder @event)
        {
            if (!Items.Any())
            {
                UserId = @event.UserId;
            }

            Items.Add(new OrderItem(@event.Sku) {Quantity = @event.Quantity, Price = @event.Price});
        }

        private void Handle(ProductQuantityChanged @event)
        {
            if (TryGetOrderItem(@event.LineItemId, out var item))
            {
                item.Quantity = @event.To;
            }
        }
    }
}
