using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;


namespace UI.Gateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class QueryControllerBase : Controller
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

    

    internal static class HttpExtensions
    {
        public static async Task<ActionResult> AsResultAsync<T>(this HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                var result = await message.ResultAsync<T>();
                return new ObjectResult(result);
            }

            return new StatusCodeResult((int)message.StatusCode);
        }

        public static async Task<T> ResultAsync<T>(this HttpResponseMessage message)
        {
            var jsonContent = await message.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(jsonContent,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            return result;
        }
    }
}
