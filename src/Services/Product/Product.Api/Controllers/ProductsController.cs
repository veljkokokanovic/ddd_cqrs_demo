using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Product.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public ActionResult Get(string category)
        {
            return Ok(ProductList.All.Where(p => string.Equals(p.Category, category, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
