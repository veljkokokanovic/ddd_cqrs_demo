using System;
using System.Collections.Generic;
using System.Linq;
using domainD;
using Order.Events;
using SharedKernel;

namespace Order.Domain
{
    public class Order : AggregateRoot
    {
        protected Order(Guid id) : base(id)
        {
        }

        private HashSet<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        private Guid UserId { get; set; }

        private bool IsPlaced { get; set; }

        private bool IsComplete { get; set; }

        private bool IsCancelled { get; set; }

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

            EnsureModify();

            if (TryGetOrderItem(sku, out var item))
            {
                RaiseEvent(new ProductQuantityChanged(sku, item.Quantity, item.Quantity + quantity));
            }
            else
            {
                RaiseEvent(new ProductAddedToOrder(sku, quantity, price, UserId));
            }
        }


        public void Place(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery));
            }

            if (IsCancelled || IsComplete)
            {
                throw new InvalidOperationException($"Order can not be placed. It is cancelled or complete.");
            }

            if (!IsPlaced)
            {
                RaiseEvent(new OrderPlaced(delivery));
            }
        }

        public void Complete()
        {
            if (!IsPlaced || IsCancelled)
            {
                throw new InvalidOperationException($"Order can not be completed. It is not placed or cancelled.");
            }

            if (!IsComplete)
            {
                RaiseEvent(new OrderCompleted());
            }
        }

        public void Cancel()
        {
            if (IsComplete)
            {
                throw new InvalidOperationException($"Order can not be cancelled. It is completed.");
            }

            if (!IsCancelled)
            {
                RaiseEvent(new OrderCancelled());
            }
        }

        public bool TryGetOrderItem(string sku, out OrderItem item)
        {
            item = Items.SingleOrDefault(i => string.Equals(i.Identity, sku));
            return item != null;
        }

        private void EnsureModify()
        {
            if (IsCancelled || IsComplete || IsPlaced)
            {
                throw new InvalidOperationException($"Can not modify order.");
            }
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
            if (TryGetOrderItem(@event.Sku, out var item))
            {
                item.Quantity = @event.To;
            }
        }

        private void Handle(OrderPlaced @event)
        {
            IsPlaced = true;
        }

        private void Handle(OrderCancelled @event)
        {
            IsCancelled = true;
        }

        private void Handle(OrderCompleted @event)
        {
            IsComplete = true;
        }

        private void Handle(ProductRemoved @event)
        {
            EnsureModify();
            Items.RemoveWhere(i => i.Identity == @event.Sku);
        }
    }
}
