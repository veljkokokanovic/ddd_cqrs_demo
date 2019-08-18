using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Gateway.Attributes;
using UI.Gateway.Models.Order.Commands;
using UI.Gateway.Models.Product;

namespace UI.Gateway.Controllers
{
    [CommandRoute("api/orders")]
    public class OrderCommandController : CommandControllerBase
    {
        public OrderCommandController(IHttpClientFactory clientFactory, IBus bus, IMapper mapper) 
            : base(clientFactory, bus, mapper)
        {
        }

        public async Task<ActionResult> Post(AddProductToOrderViewModel model)
        {
            var productsResponse = await GetAsync(HttpClients.ProductApi, "products");
            var products = await productsResponse.ResultAsync<IEnumerable<ProductViewModel>>();
            var command = CommandMapper.Map<AddProductToOrder>(model);
            command.Price = products.First(p => p.Sku == command.Sku).Price;
            await Bus.Send(command);
            return Accepted();
        }

        public async Task<ActionResult> Post(CancelOrderViewModel model)
        {
            await SendCommandAsync<CancelOrder>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(SetProductQuantityViewModel model)
        {
            await SendCommandAsync<SetProductQuantity>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(PlaceOrderViewModel model)
        {
            await SendCommandAsync<PlaceOrder>(model);
            return Accepted();
        }

        public async Task<ActionResult> Post(RemoveProductViewModel model)
        {
            await SendCommandAsync<RemoveProduct>(model);
            return Accepted();
        }
    }
}
