﻿using System;
using System.Collections.Generic;
using System.Text;
using domainD;
using SharedKernel;

namespace Order.Events
{
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(Delivery deliveryInfo, Guid userId)
        {
            DeliveryInfo = deliveryInfo;
            UserId = userId;
        }

        public Guid UserId { get; set; }

        public Delivery DeliveryInfo { get; set; }
    }
}
