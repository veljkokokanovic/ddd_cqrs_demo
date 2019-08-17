using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using UI.Gateway.Models.Order;
using UI.Gateway.Models.Product;

namespace UI.Gateway.Controllers
{
    [Route("api/orders")]
    public class OrderQueryController : QueryControllerBase
    {
        public OrderQueryController(IHttpClientFactory clientFactory, IMapper mapper) 
            : base(clientFactory, mapper)
        {
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var orderTask = GetAsync(HttpClients.OrderApi, $"orders/{id}");
            var deliveryTask = GetAsync(HttpClients.DeliveryApi, $"deliveries?oid={id}");
            var productsTask = GetAsync(HttpClients.ProductApi, "products");
            var orderModel = await (await orderTask).ResultAsync<ReadModel.Order.Order>();

            var oderViewModel = ModelMapper.Map<OrderViewModel>(orderModel);

            var deliveryModel = await (await deliveryTask).ResultAsync<IEnumerable<ReadModel.Delivery.Order>>();
            ModelMapper.Map(deliveryModel.FirstOrDefault(), oderViewModel);
            var productsViewModel = await (await productsTask).ResultAsync<IEnumerable<ProductViewModel>>();
            foreach(var productViewModel in oderViewModel.Products)
            {
                var product = productsViewModel.First(p => p.Sku == productViewModel.Sku);
                productViewModel.Category = product.Category;
                productViewModel.Name = product.Name;
            }

            return Ok(oderViewModel);
        }

        [HttpGet("")]
        public async Task<ActionResult> GetUserOrdersAsync([FromQuery] Guid userId, [FromQuery] OrderStatus? status)
        {
            var productsTask = GetAsync(HttpClients.ProductApi, "products");
            var ordersQuery = $"orders?userId={userId}{(status.HasValue ? "&status=" + status.Value : "")}";
            var ordersRequest = await GetAsync(HttpClients.OrderApi, ordersQuery);
            var orders = await ordersRequest.ResultAsync<IEnumerable<ReadModel.Order.Order>>();
            var deliveriesResponse = await GetAsync(HttpClients.DeliveryApi, $"deliveries?{string.Join("&", orders.Select(o => "oid="+o.Id))}");
            var deliveries = await deliveriesResponse.ResultAsync<IEnumerable<ReadModel.Delivery.Order>>();
            var ordersViewModel = new List<OrderViewModel>();
            var productsViewModel = await (await productsTask).ResultAsync<IEnumerable<ProductViewModel>>();

            foreach (var order in orders)
            {
                var orderViewModel = ModelMapper.Map<OrderViewModel>(order);
                var delivery = deliveries.FirstOrDefault(d => d.ReferenceOrderId == order.Id);
                ModelMapper.Map(delivery, orderViewModel);
                foreach (var productViewModel in orderViewModel.Products)
                {
                    var product = productsViewModel.First(p => p.Sku == productViewModel.Sku);
                    productViewModel.Category = product.Category;
                    productViewModel.Name = product.Name;
                }

                ordersViewModel.Add(orderViewModel);
            }

            return Ok(ordersViewModel);
        }
    }
}