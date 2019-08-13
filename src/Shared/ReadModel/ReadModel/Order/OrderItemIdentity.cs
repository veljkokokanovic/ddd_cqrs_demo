using System;
using System.Collections.Generic;
using System.Text;

namespace ReadModel.Order
{
    public class OrderItemIdentity
    {
        public string Sku { get; set; }

        public Guid OrderId { get; set; }
    }
}
