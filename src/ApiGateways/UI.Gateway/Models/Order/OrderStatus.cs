using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Gateway.Models.Order
{
    public enum OrderStatus
    {
        Pending,
        Placed,
        Cancelled,
        Delivering,
        Delivered,
        Returned
    }
}
