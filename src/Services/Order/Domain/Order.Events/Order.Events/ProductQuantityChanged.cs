using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Order.Events
{
    public class ProductQuantityChanged : DomainEvent
    {
        public string LineItemId { get; set; }

        public int From { get; set; }

        public int To { get; set; }
    }
}
