using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Events
{
    public class OrderDelivered : DomainEvent
    {
        public OrderDelivered(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}
