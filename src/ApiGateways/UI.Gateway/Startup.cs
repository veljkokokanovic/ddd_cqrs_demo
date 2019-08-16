using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Delivery.Commands;
using GreenPipes;
using MassTransit.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Order.Commands;
using PlaceOrder = Order.Commands.PlaceOrder;

namespace UI.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }

        private ILoggerFactory LoggerFactory { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.Converters.Add(new StringEnumConverter(true)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2).Services
                .AddHttpClient(HttpClients.ProductApi, client =>
                {
                    client.BaseAddress = new Uri(Configuration[HttpClients.ProductApi]);
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                }).Services
                .AddHttpClient(HttpClients.OrderApi, client =>
                {
                    client.BaseAddress = new Uri(Configuration[HttpClients.OrderApi]);
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                }).Services
                .AddHttpClient(HttpClients.DeliveryApi, client =>
                {
                    client.BaseAddress = new Uri(Configuration[HttpClients.DeliveryApi]);
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                }).Services
                .AddHttpContextAccessor()
                .AddMassTransit(ConfigureBus);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void ConfigureBus(IServiceCollectionConfigurator configurator)
        {
            configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var busConfig = Configuration.GetSection("Bus");
                var host = cfg.ConfigureHost(busConfig);
                cfg.ConfigureSend(sendPipe =>
                {
                    sendPipe.UseSendFilter(new OperationContextEstablisher(provider));
                });

                EndpointConvention.Map<AddProductToOrder>(new Uri(new Uri(busConfig["Host"]), nameof(AddProductToOrder)));
                EndpointConvention.Map<CancelOrder>(new Uri(new Uri(busConfig["Host"]), nameof(CancelOrder)));
                EndpointConvention.Map<PlaceOrder>(new Uri(new Uri(busConfig["Host"]), nameof(PlaceOrder)));
                EndpointConvention.Map<RemoveProduct>(new Uri(new Uri(busConfig["Host"]), nameof(RemoveProduct)));
                EndpointConvention.Map<SetProductQuantity>(new Uri(new Uri(busConfig["Host"]), nameof(SetProductQuantity)));

                EndpointConvention.Map<StartDelivery>(new Uri(new Uri(busConfig["Host"]), nameof(StartDelivery)));
                EndpointConvention.Map<ReturnOrder>(new Uri(new Uri(busConfig["Host"]), nameof(ReturnOrder)));
                EndpointConvention.Map<DeliverOrder>(new Uri(new Uri(busConfig["Host"]), nameof(DeliverOrder)));
            }));
        }
    }

    public class OperationContextEstablisher : IFilter<SendContext>
    {
        private IServiceProvider _serviceProvider;

        public OperationContextEstablisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Send(SendContext context, IPipe<SendContext> next)
        {
            var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext.Response.Headers.ContainsKey(KnownHeaders.CorrelationId))
            {
                context.Headers.Set(MassTransit.Extensions.KnownHeaders.CorrelationId, httpContext.Response.Headers[KnownHeaders.CorrelationId].FirstOrDefault());
            }

            context.Headers.Set(MassTransit.Extensions.KnownHeaders.UserId, "11111111-1111-1111-1111-111111111111");

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope(nameof(OperationContextEstablisher));
        }
    }
}
