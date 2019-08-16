using System;
using domainD;

namespace Delivery.Commands
{
    public class PlaceOrder : ICommand
    {
        public Guid ReferenceOrderId { get; set; }

        public Guid UserId { get; set; }

        public string Address { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string PhoneNumber { get; set; }
    }
}
