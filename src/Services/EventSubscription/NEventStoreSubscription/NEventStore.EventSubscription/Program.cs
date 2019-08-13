using System;
using System.Threading.Tasks;
using Console.Host;
using domainD.EventSubscription;
using domainD.EventSubscription.NEventStore;
using Microsoft.Extensions.Hosting;
using Order.Events;

namespace NEventStore.EventSubscription
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .AddConfigFile()
                .AddEventSubscription<NEventStoreEventSubscription>(cfg => cfg
                    .UseFileCheckpointLoader()
                    .On<ProductAddedToOrder>().HandleAsync(HandleOrderEvent.ProductAddedToOrder)
                    .On<ProductQuantityChanged>().HandleAsync(HandleOrderEvent.ProductQuantityChanged))
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .ConfigureServices(ServicesConfigurator.ConfigureEventStore)
                .ConfigureServices(ServicesConfigurator.ConfigureEventSubscriptionServices)
                .RunConsoleAsync();
        }
    }
}
