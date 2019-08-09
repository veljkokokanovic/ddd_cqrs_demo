using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Gateway.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.Gateway.Controllers
{
    public class ProductsController : ControllerBase
    {
        public ProductsController(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Get(string category)
        {
            var response = await GetAsync(HttpClients.ProductApi, $"products?category={category}");
            return await response.ResultAsync<IEnumerable<ProductViewModel>>();
        }
    }
}
