using ReadModel.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.Gateway.Models.Order
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public ICollection<OrderItemViewModel> Products { get; set; }

        public DateTime PlacedOn { get; set; }

        public OrderStatus Status { get; set; }

        public string Comment { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime? DeliveryStartedAt { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public decimal Total => Products.Sum(p => p.Price * p.Quantity);
    }
}
