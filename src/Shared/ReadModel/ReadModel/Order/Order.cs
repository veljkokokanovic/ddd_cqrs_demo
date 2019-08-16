using System;
using System.Collections.Generic;

namespace ReadModel.Order
{
    public class Order : Entity<Guid>
    {
        public Guid UserId { get; set; }

        public ICollection<OrderItem> Products { get; set; }

        public DateTime PlacedOn { get; set; }

        public OrderStatus Status { get; set; }

        public string Comment { get; set; }
    }
}
