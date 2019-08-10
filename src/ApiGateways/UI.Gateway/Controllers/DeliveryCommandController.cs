using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Order.Commands;
using AddProductToOrder = Order.Commands.AddProductToOrder;

namespace UI.Gateway.Controllers
{
    [ApiController]
    [CommandRoute("api/deliveries")]
    public class DeliveryCommandController : Controller
    {
        
    }
}
