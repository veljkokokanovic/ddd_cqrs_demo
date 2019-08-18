using AutoMapper;
using Delivery.Commands;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Delivery.Commands;

namespace UI.Gateway.Controllers
{
    [CommandRoute("api/deliveries")]
    public class DeliveryCommandController : CommandControllerBase
    {
        public DeliveryCommandController(IHttpClientFactory clientFactory, IBus bus, IMapper mapper) 
            : base(clientFactory, bus, mapper)
        {
        }

        public async Task<ActionResult> Post(StartDeliveryViewModel model)
        {
            await SendCommandAsync<StartDelivery>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(ReturnOrderViewModel model)
        {
            await SendCommandAsync<ReturnOrder>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(DeliverOrderViewModel model)
        {
            await SendCommandAsync<DeliverOrder>(model);
            return Accepted();
        }
    }
}
