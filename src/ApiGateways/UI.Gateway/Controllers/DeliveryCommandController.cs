using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Delivery.Commands;

namespace UI.Gateway.Controllers
{
    [ApiController]
    [CommandRoute("api/deliveries")]
    [EstablishCommandContext]
    public class DeliveryCommandController : Controller
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public DeliveryCommandController(IBus bus, IMapper mapper)
        {
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<ActionResult> Post(StartDeliveryViewModel model)
        {
            await _bus.Send(_mapper.Map<Delivery.Commands.StartDelivery>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(ReturnOrderViewModel model)
        {
            await _bus.Send(_mapper.Map<Delivery.Commands.ReturnOrder>(model));
            return Accepted();
        }

        public async Task<ActionResult> Post(DeliverOrderViewModel model)
        {
            await _bus.Send(_mapper.Map<Delivery.Commands.DeliverOrder>(model));
            return Accepted();
        }
    }
}
