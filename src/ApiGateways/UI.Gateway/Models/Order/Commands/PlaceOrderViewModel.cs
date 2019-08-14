using System;
using domainD;

namespace UI.Gateway.Models.Order.Commands
{
    public class PlaceOrderViewModel
    {
        public Guid OrderId { get; set; }

        public string Address { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string PhoneNumber { get; set; }
    }
}
