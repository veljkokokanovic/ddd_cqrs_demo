using System;
using domainD;

namespace UI.Gateway.Models.Delivery.Commands
{
    public class ReturnOrderViewModel
    {
        public Guid OrderId { get; set; }

        public string Reason { get; set; }
    }
}
