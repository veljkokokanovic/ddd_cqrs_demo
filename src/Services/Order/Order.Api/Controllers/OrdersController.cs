using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadModel.Order;
using ReadModel.Repository.MsSql;

namespace Order.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Route("{id}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        public async Task<ActionResult> GetUserOrdersAsync([FromQuery] Guid userId, [FromQuery] OrderStatus? status = null)
        {
            var order = await _orderRepository.GetUserOrdersAsync(userId, status);
            return Ok(order);
        }
    }
}
