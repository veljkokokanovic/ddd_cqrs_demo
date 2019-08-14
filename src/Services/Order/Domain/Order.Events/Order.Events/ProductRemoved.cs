using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using domainD;

namespace Order.Events
{
    public class ProductRemoved : DomainEvent
    {
        public ProductRemoved(string sku)
        {
            Sku = sku;
        }

        public string Sku { get; set; }
    }
}
