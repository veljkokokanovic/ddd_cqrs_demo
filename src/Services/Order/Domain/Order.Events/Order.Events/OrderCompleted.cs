using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Order.Events
{
    public class OrderCompleted : DomainEvent
    {
        public OrderCompleted(bool success = true, string comment = null)
        {
            Success = success;
            Comment = comment;
        }

        public bool Success { get; set; }

        public string Comment { get; set; }
    }
}
