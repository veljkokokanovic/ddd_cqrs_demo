using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Order.Commands
{
    public class CompleteOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}
