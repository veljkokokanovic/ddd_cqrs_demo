using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Order.Commands
{
    public class PlaceOrder : ICommand
    {
        public Guid OrderId { get; set; }

        public string Address { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string PhoneNumber { get; set; }
    }
}
