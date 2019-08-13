using System;
using System.Collections.Generic;
using System.Text;
using domainD;
using Order.Events;

namespace Order.Domain
{
    public class OrderItem : Entity<string>
    {
        internal OrderItem(string id) : base(id)
        {
        }

        internal int Quantity { get; set; }

        internal decimal Price { get; set; }

        public void ChangeQuantity(int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentException("Quantity must be > 0", nameof(quantity));
            }

            RaiseEvent(new ProductQuantityChanged{LineItemId = Identity, From = Quantity, To = quantity});
        }
    }
}
