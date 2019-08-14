using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Events
{
    public class OrderReturned : DomainEvent
    {
        public OrderReturned(Guid userId, string reason)
        {
            Reason = reason;
            UserId = userId;
        }

        public string Reason { get; set; }

        private Guid UserId { get; set; }
    }
}
