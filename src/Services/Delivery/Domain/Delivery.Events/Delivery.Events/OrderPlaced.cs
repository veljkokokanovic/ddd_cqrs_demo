using System;
using domainD;

namespace Delivery.Events
{
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(SharedKernel.Delivery deliveryInfo, Guid userId)
        {
            DeliveryInfo = deliveryInfo;
            UserId = userId;
        }

        public Guid UserId { get; set; }

        public SharedKernel.Delivery DeliveryInfo { get; set; }
    }
}
