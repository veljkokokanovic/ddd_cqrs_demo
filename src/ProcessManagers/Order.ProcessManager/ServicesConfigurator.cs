using GreenPipes;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.Extensions;
using MassTransit.Extensions.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Order.Commands;
using Order.ProcessManager.Sagas;
using Order.ProcessManager.Sagas.Persistence;
using System;
using System.Data.SqlClient;

namespace Order.ProcessManager
{
    public static class ServicesConfigurator
    {
        public static void ConfigureBus(HostBuilderContext context, IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var busConfig = context.Configuration.GetSection("Bus");
                    var host = cfg.ConfigureHost(busConfig);

                    var connectionString = provider.GetRequiredService<IConfiguration>()["SagaStoreConnection"];

                    var optionsBuilder = new DbContextOptionsBuilder<SagaDbContext<SagaInstance, SagaInstanceMap>>();
                    optionsBuilder.UseSqlServer(connectionString, options => options.CommandTimeout(5));
                    DbContext contextFactory() => new SagaDbContext<SagaInstance, SagaInstanceMap>(optionsBuilder.Options);
                    var repository = EntityFrameworkSagaRepository<SagaInstance>.CreateOptimistic(contextFactory);

                    cfg.ReceiveEndpoint(host, "OrderWorkflow", e =>
                    {
                        e.UseRetry(x =>
                        {
                            x.Handle<DbUpdateConcurrencyException>();
                            // This is the SQLServer error code for duplicate key, if you are using another Relational Db, the code might be different
                            x.Handle<DbUpdateException>(y => y.InnerException is SqlException ex && ex.Number == 2627);
                            x.Interval(5, TimeSpan.FromMilliseconds(100));
                        });
                        e.UseFilter(new OperationContextFilter());
                        e.StateMachineSaga(new OrderSaga(provider.GetRequiredService<ILogger<OrderSaga>>()), repository);
                    });

                    EndpointConvention.Map<CompleteOrder>(new Uri(new Uri(busConfig["Host"]), nameof(CompleteOrder)));
                    EndpointConvention.Map<FailOrder>(new Uri(new Uri(busConfig["Host"]), nameof(FailOrder)));
                    EndpointConvention.Map<Delivery.Commands.PlaceOrder>(new Uri(new Uri(busConfig["Host"]), "CreateDelivery"));

                    contextFactory().Database.EnsureCreated();
                }));
            });

            services.AddSingleton<IHostedService, BusService>();
        }
    }
}
