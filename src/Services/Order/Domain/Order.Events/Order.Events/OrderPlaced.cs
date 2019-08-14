using System;
using System.Collections.Generic;
using System.Text;
using domainD;
using SharedKernel;

namespace Order.Events
{
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(Delivery deliveryInfo)
        {
            DeliveryInfo = deliveryInfo;
        }

        public Delivery DeliveryInfo { get; set; }
    }
}
