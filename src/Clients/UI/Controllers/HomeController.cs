using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> MyOrder()
        {
            var products = await ResultAsync("products");
            var orders = await ResultAsync("orders?userId=11111111-1111-1111-1111-111111111111&status=placed");

            dynamic model = new ExpandoObject();
            model.Pizzas = ((IEnumerable)products).Cast<dynamic>().Where(p => p.category == "Pizza");
            model.Drinks = ((IEnumerable)products).Cast<dynamic>().Where(p => p.category == "Drink");
            model.Order = ((JArray)orders).SingleOrDefault();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<dynamic> ResultAsync(string endpoint)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_configuration["ApiGateway"]);
            using (var response = await client.GetAsync(endpoint))
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                return JToken.Parse(jsonContent);
            }
        }
    }
}
