using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Commands
{
    public class DeliverOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}
