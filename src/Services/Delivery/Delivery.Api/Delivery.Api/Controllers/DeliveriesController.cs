using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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

        [MatchQueryParam("oid")]

        public async Task<ActionResult> GetFromOrderIdsAsync([FromQuery] Guid[] oid)
        {
            var orders = await _deliveryRepository.GetFromOrderIdsAsync(oid);
            return Ok(orders);
        }

        [MatchQueryParam]
        public async Task<ActionResult> GetAllAsync()
        {
            var orders = await _deliveryRepository.GetAllAsync();
            return Ok(orders);
        }
    }
    public class MatchQueryParamAttribute : Attribute, IActionConstraint
    {
        private readonly string[] keys;

        public MatchQueryParamAttribute(params string[] keys)
        {
            this.keys = keys;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var query = context.RouteContext.HttpContext.Request.Query;
            var result = query.Count == keys.Length && keys.All(key => query.ContainsKey(key));
            return result;
        }
    }
}
