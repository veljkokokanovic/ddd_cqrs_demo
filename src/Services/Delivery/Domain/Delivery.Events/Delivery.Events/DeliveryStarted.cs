using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Events
{
    public class DeliveryStarted : DomainEvent
    {
        public DeliveryStarted(Guid userId, Guid referenceOrderId)
        {
            UserId = userId;
            ReferenceOrderId = referenceOrderId;
        }

        public Guid UserId { get; set; }

        public Guid ReferenceOrderId { get; set; }
    }
}
