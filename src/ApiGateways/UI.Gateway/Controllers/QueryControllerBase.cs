using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;


namespace UI.Gateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public abstract class QueryControllerBase : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public QueryControllerBase(IHttpClientFactory clientFactory, IMapper mapper)
        {
            _clientFactory = clientFactory;
            ModelMapper = mapper;
        }

        protected async Task<HttpResponseMessage> GetAsync(string clientName, string endpoint)
        {
            var client = _clientFactory.CreateClient(clientName);
            return await client.GetAsync(endpoint);
        }

        protected IMapper ModelMapper { get; private set; }
    }
}
