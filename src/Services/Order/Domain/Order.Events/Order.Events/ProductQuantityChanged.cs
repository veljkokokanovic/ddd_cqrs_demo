using System;
using System.Collections.Generic;
using System.Text;
using domainD;

namespace Order.Events
{
    public class ProductQuantityChanged : DomainEvent
    {
        public ProductQuantityChanged(string sku, int from, int to)
        {
            Sku = sku;
            From = from;
            To = to;
        }

        public string Sku { get; set; }

        public int From { get; set; }

        public int To { get; set; }
    }
}
