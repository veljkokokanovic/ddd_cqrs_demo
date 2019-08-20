using AutoMapper;
using domainD;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Gateway.Attributes;

namespace UI.Gateway.Controllers
{
    [ApiController]
    [EstablishCommandContext]
    public abstract class CommandControllerBase : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CommandControllerBase(IHttpClientFactory clientFactory, IBus bus, IMapper mapper)
        {
            Bus = bus;
            CommandMapper = mapper;
            _clientFactory = clientFactory;
        }

        protected IBus Bus { get; private set; }

        protected IMapper CommandMapper { get; private set; }

        protected async Task<HttpResponseMessage> GetAsync(string clientName, string endpoint)
        {
            var client = _clientFactory.CreateClient(clientName);
            return await client.GetAsync(endpoint);
        }

        protected Task SendCommandAsync<TCommand>(object viewModel)
            where TCommand : class, ICommand
        {
            return Bus.Send(CommandMapper.Map<TCommand>(viewModel));
        }
    }
}
