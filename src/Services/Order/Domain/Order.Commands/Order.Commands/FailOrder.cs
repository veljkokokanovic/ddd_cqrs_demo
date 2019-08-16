using domainD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Commands
{
    public class FailOrder : ICommand
    {
        public Guid OrderId { get; set; }

        public string Reason { get; set; }
    }
}
