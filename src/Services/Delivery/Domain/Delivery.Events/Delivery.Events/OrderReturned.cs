using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Events
{
    public class OrderReturned : DomainEvent
    {
        public OrderReturned(Guid userId, string reason, Guid referenceOrderId)
        {
            Reason = reason;
            UserId = userId;
            ReferenceOrderId = referenceOrderId;
        }

        public string Reason { get; set; }

        private Guid UserId { get; set; }

        public Guid ReferenceOrderId { get; set; }
    }
}
