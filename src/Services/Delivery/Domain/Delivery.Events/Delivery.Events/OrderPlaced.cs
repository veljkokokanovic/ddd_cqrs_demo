using System;
using domainD;

namespace Delivery.Events
{
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(SharedKernel.Delivery deliveryInfo, Guid userId, Guid referenceOrderId)
        {
            DeliveryInfo = deliveryInfo;
            UserId = userId;
            ReferenceOrderId = referenceOrderId;
        }

        public Guid UserId { get; set; }

        public Guid ReferenceOrderId { get; set; }

        public SharedKernel.Delivery DeliveryInfo { get; set; }
    }
}
