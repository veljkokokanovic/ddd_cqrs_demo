using CommandHandler.Host;
using domainD;
using domainD.Repository.NEventStore;
using GreenPipes;
using MassTransit;
using MassTransit.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NEventStore;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using NEventStore.Serialization.Json;
using Order.CommandHandler.CommandConsumers;
using Order.Commands;
using System;
using System.Data.SqlClient;
using System.Linq;
using CommandHandler.Host.Filters;

namespace Order.CommandHandler
{
    public static class ServicesConfigurator
    {
        public static void ConfigureBus(HostBuilderContext context, IServiceCollection services )
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<AddProductToOrderConsumer>();

                config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.ConfigureHost(context.Configuration.GetSection("Bus"));

                    cfg.ReceiveEndpoint(host, nameof(AddProductToOrder), e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, 100));
                        e.ConfigureConsumer<AddProductToOrderConsumer>(provider);
                        e.UseFilter(new OperationContextFilter());
                    });
                }));
            });

            services.AddSingleton<IHostedService, BusService>();
        }

        public static void ConfigureDomain(HostBuilderContext context, IServiceCollection services)
        {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract))
            {
                foreach (var i in type.GetInterfaces())
                {
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                    {
                        var interfaceType = typeof(ICommandHandler<>).MakeGenericType(i.GetGenericArguments());
                        services.AddTransient(interfaceType, type);
                    }
                }
            }
        }

        public static void ConfigureEventStore(HostBuilderContext context, IServiceCollection services)
        {
            services.AddNEventStore(cfg =>
            {
                cfg.UsingSqlPersistence(new NetStandardConnectionFactory(SqlClientFactory.Instance,
                        context.Configuration["EventStoreConnection"]))
                    .EnlistInAmbientTransaction()
                    .WithDialect(new MsSqlDialect())
                    .UsingJsonSerialization();
            });
        }
    }
}
