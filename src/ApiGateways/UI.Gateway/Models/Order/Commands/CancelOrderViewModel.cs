using System;
using domainD;

namespace UI.Gateway.Models.Order.Commands
{
    public class CancelOrderViewModel
    {
        public Guid OrderId { get; set; }
    }
}
