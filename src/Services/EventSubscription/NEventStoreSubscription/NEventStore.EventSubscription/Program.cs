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
                    .On<ProductAddedToOrder>().HandleAsync(HandlerFor.ProductAddedToOrder)
                    .On<ProductQuantityChanged>().HandleAsync(HandlerFor.ProductQuantityChanged)
                    .On<OrderCancelled>().HandleAsync(HandlerFor.OrderCancelled)
                    .On<OrderCompleted>().HandleAsync(HandlerFor.OrderCompleted)
                    .On<OrderPlaced>().HandleAsync(HandlerFor.OrderPlaced)
                    .On<ProductRemoved>().HandleAsync(HandlerFor.ProductRemoved))
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .ConfigureServices(ServicesConfigurator.ConfigureEventStore)
                .ConfigureServices(ServicesConfigurator.ConfigureEventSubscriptionServices)
                .RunConsoleAsync();
        }
    }
}
