using AutoMapper;
using domainD.Repository.NEventStore;
using MassTransit;
using MassTransit.Extensions;
using MassTransit.Extensions.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using NEventStore.Serialization.Json;
using ReadModel.Repository.MsSql;
using System.Data.SqlClient;

namespace NEventStore.EventSubscription
{
    public static class ServicesConfigurator
    {
        public static void ConfigureBus(HostBuilderContext context, IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseInMemoryOutbox();
                    var host = cfg.ConfigureHost(context.Configuration.GetSection("Bus"));
                    cfg.ConfigurePublish(p => p.AddPipeSpecification(new PublishContextEstablisher()));
                }));
            });

            services.AddSingleton<IHostedService, BusService>();
        }

        public static void ConfigureEventStore(HostBuilderContext context, IServiceCollection services)
        {
            services.AddNEventStore(cfg =>
            {
                cfg.UsingSqlPersistence(new NetStandardConnectionFactory(SqlClientFactory.Instance,
                        context.Configuration["EventStoreConnection"]))
                    .WithDialect(new MsSqlDialect())
                    .UsingJsonSerialization();
            });
        }

        public static void ConfigureEventSubscriptionServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { cfg.AddProfile<ModelMappingProfile>(); },
                typeof(ServicesConfigurator).Assembly);
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IDeliveryRepository, DeliveryRepository>();
        }
    }
}
