using System;
using System.Linq;
using Console.Host;
using Console.Host.Filters;
using domainD;
using GreenPipes;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.ProcessManager.Sagas.Order;

namespace Order.ProcessManager
{
    public static class ServicesConfigurator
    {
        public static void ConfigureBus(HostBuilderContext context, IServiceCollection services )
        {
            services.AddMassTransit(config =>
            {
                config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.ConfigureHost(context.Configuration.GetSection("Bus"));

                    ISagaDbContextFactory<SagaInstance> contextFactory = () => new SagaDbContext<SagaInstance, SagaInstanceMap>("");

                    // For Optimistic
                    var repository = new EntityFrameworkSagaRepository<SagaInstance>().CreateOptimistic(contextFactory);

                    cfg.ReceiveEndpoint(host, "OrderWorkflow", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, 100));
                        e.ConfigureConsumer<StartDeliveryConsumer>(provider);
                        e.UseFilter(new OperationContextFilter());
                    });
                }));
            });

            services.AddSingleton<IHostedService, BusService>();
        }
    }
}
