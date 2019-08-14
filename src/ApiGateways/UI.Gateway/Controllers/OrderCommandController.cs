using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Order.Commands;

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

        public async Task<ActionResult> Post(AddProductToOrderViewModel model)
        {
            await _bus.Send(_mapper.Map<Order.Commands.AddProductToOrder>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(CancelOrderViewModel model)
        {
            await _bus.Send(_mapper.Map<Order.Commands.CancelOrder>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(SetProductQuantityViewModel model)
        {
            await _bus.Send(_mapper.Map<Order.Commands.SetProductQuantity>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(PlaceOrderViewModel model)
        {
            await _bus.Send(_mapper.Map<Order.Commands.PlaceOrder>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(RemoveProductViewModel model)
        {
            await _bus.Send(_mapper.Map<Order.Commands.RemoveProduct>(model));
            return Accepted();
        }
    }
}
