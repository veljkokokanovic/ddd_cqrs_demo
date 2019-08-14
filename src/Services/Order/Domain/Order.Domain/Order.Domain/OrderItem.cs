using domainD;
using Order.Events;
using System;

namespace Order.Domain
{
    public class OrderItem : Entity<string>
    {
        internal OrderItem(string id) : base(id)
        {
        }

        internal int Quantity { get; set; }

        internal decimal Price { get; set; }

        public void SetQuantity(int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentException("Quantity must be > 0", nameof(quantity));
            }

            if (quantity != Quantity)
            {
                RaiseEvent(new ProductQuantityChanged(Identity, Quantity, quantity));
            }
        }

        public void Remove()
        {
            RaiseEvent(new ProductRemoved(Identity));
        }
    }
}
