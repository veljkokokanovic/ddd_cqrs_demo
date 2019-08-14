using System;
using System.Collections.Generic;
using ReadModel.Order;

namespace ReadModel.Delivery
{
    public class Order : Entity<Guid>
    {
        public Guid UserId { get; set; }

        public Guid ReferenceOrderId { get; set; }

        public DateTime PlacedOn { get; set; }

        public DateTime? DeliveryStartedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public Address DeliveryAddress { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string PhoneNumber { get; set; }

        public DeliveryStatus Status { get; set; }
    }
}
