﻿using System;
using domainD;

namespace Order.Commands
{
    public class AddProductToOrder : ICommand
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public decimal Price { get; set; }
    }
}
