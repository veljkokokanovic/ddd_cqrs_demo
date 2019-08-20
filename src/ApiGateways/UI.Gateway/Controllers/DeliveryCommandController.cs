using AutoMapper;
using Delivery.Commands;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Delivery;
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
            model.OrderId = await GetDeliveryId(model.OrderId);
            await SendCommandAsync<StartDelivery>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(ReturnOrderViewModel model)
        {
            model.OrderId = await GetDeliveryId(model.OrderId);
            await SendCommandAsync<ReturnOrder>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(DeliverOrderViewModel model)
        {
            model.OrderId = await GetDeliveryId(model.OrderId);
            await SendCommandAsync<DeliverOrder>(model);
            return Accepted();
        }

        private async Task<Guid> GetDeliveryId(Guid orderId)
        {
            var deliveriesResponse = await GetAsync(HttpClients.DeliveryApi, $"deliveries?oid={orderId}");
            var deliveries = await deliveriesResponse.ResultAsync<IEnumerable<dynamic>>();
            return Guid.Parse(deliveries.First().Value<string>("id"));
        }
    }
}
