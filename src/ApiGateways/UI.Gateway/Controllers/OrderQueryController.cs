using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UI.Gateway.Controllers
{
    [Route("api/orders")]
    public class OrderQueryController : QueryControllerBase
    {
        public OrderQueryController(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}