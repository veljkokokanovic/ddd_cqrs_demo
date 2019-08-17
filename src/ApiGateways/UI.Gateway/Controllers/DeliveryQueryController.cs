using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace UI.Gateway.Controllers
{
    [Route("api/deliveries")]
    public class DeliveryQueryController : QueryControllerBase
    {
        public DeliveryQueryController(IHttpClientFactory clientFactory, IMapper mapper) 
            : base(clientFactory, mapper)
        {
        }
    }
}