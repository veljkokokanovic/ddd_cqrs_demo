using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Gateway.Models;
using UI.Gateway.Models.Product;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.Gateway.Controllers
{
    [Route("api/products")]
    public class ProductsController : QueryControllerBase
    {
        public ProductsController(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
        }

        public async Task<ActionResult> Get(string category)
        {
            var response = await GetAsync(HttpClients.ProductApi, $"products?category={category}");
            return await response.ResultAsync<IEnumerable<ProductViewModel>>();
        }
    }
}
