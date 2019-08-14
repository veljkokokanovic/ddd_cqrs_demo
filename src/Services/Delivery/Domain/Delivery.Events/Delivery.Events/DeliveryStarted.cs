using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Events
{
    public class DeliveryStarted : DomainEvent
    {
        public DeliveryStarted(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}
