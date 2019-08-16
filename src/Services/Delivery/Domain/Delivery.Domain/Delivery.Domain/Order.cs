using System;
using Delivery.Events;
using domainD;

namespace Delivery.Domain
{
    public class Order : AggregateRoot
    {
        protected Order(Guid id) : base(id)
        {
        }

        private Guid UserId { get; set; }

        private Guid ReferenceOrderId { get; set; }

        private bool IsComplete { get; set; }

        private bool HasDeliveryStarted { get; set; }

        public void StartDelivery()
        {
            if (IsComplete)
            {
                throw new InvalidOperationException($"Can not start delivery on completed order.");
            }

            if (!HasDeliveryStarted)
            {
                RaiseEvent(new DeliveryStarted(UserId, ReferenceOrderId));
            }
        }

        public void Deliver()
        {
            if (!HasDeliveryStarted)
            {
                throw new InvalidOperationException($"Can not deliver order that hasnt started delivery.");
            }

            if (!IsComplete)
            {
                RaiseEvent(new OrderDelivered(UserId, ReferenceOrderId));
            }
        }

        public void Return(string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new ArgumentNullException(nameof(reason));
            }

            if (!HasDeliveryStarted)
            {
                throw new InvalidOperationException($"Can not return order that hasnt started delivery.");
            }

            if (!IsComplete)
            {
                RaiseEvent(new OrderReturned(UserId, reason, ReferenceOrderId));
            }
        }

        private void Handle(OrderPlaced @event)
        {
            UserId = @event.UserId;
            ReferenceOrderId = @event.ReferenceOrderId;
        }

        private void Handle(OrderDelivered @event)
        {
            IsComplete = true;
        }

        private void Handle(OrderReturned @event)
        {
            IsComplete = true;
        }

        private void Handle(DeliveryStarted @event)
        {
            HasDeliveryStarted = true;
        }
    }
}
