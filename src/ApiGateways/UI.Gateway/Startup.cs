using AutoMapper;
using Delivery.Commands;
using MassTransit;
using MassTransit.Extensions;
using MassTransit.Extensions.Filters;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Order.Commands;
using System;
using System.Net.Http.Headers;
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

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders(KnownHeaders.CorrelationId)
                .WithOrigins("http://localhost:7000");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

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
                    sendPipe.UseSendFilter(new OperationContextEstablisher());
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
}
