using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Delivery.Commands
{
    public class ReturnOrder : ICommand
    {
        public Guid OrderId { get; set; }

        public string Reason { get; set; }
    }
}
