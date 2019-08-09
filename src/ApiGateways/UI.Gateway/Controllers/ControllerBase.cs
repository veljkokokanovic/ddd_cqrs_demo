using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace UI.Gateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class ControllerBase : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ControllerBase(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        protected async Task<HttpResponseMessage> GetAsync(string clientName, string endpoint)
        {
            var client = _clientFactory.CreateClient(clientName);
            return await client.GetAsync(endpoint);
        }
    }

    internal static class HttpExtensions
    {
        public static async Task<ActionResult> ResultAsync<T>(this HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                var jsonContent = await message.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(jsonContent,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                return new ObjectResult(result);
            }

            return new StatusCodeResult((int)message.StatusCode);
        }
    }
}
