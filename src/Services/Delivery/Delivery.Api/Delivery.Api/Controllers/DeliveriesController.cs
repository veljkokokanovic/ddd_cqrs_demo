using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadModel.Repository.MsSql;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DeliveriesController : Controller
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveriesController(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        [Route("{id}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            var order = await _deliveryRepository.GetAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        public async Task<ActionResult> GetFromOrderIdsAsync([FromQuery] Guid[] oid)
        {
            var order = await _deliveryRepository.GetFromOrderIdsAsync(oid);
            return Ok(order);
        }
    }
}
