using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Commands
{
    public class StartDelivery : ICommand
    {
        public Guid OrderId { get; set; }
    }
}
