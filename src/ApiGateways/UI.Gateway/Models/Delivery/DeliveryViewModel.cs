using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Gateway.Models.Delivery
{
    public class DeliveryViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public DateTime PlacedOn { get; set; }

        public DateTime? DeliveryStartedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public string DeliveryAddress { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Status { get; set; }
    }
}
