using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Order.Commands;
using AddProductToOrder = Order.Commands.AddProductToOrder;

namespace UI.Gateway.Controllers
{
    [ApiController]
    [CommandRoute("api/orders")]
    [EstablishCommandContext]
    public class OrderCommandController : Controller
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public OrderCommandController(IBus bus, IMapper mapper)
        {
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<ActionResult> Post(AddProductToOrder model)
        {
            await _bus.Send(_mapper.Map<Order.Commands.AddProductToOrder>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(SetProductQuantity model)
        {
            return Ok();
        }
    }
}
