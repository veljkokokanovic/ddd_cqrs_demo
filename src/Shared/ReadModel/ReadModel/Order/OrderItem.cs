using System;
using System.Collections.Generic;
using System.Text;

namespace ReadModel.Order
{
    public class OrderItem
    {
        public OrderItemIdentity Id { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
