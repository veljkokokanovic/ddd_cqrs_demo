using System;
using System.Threading.Tasks;
using Console.Host;
using domainD.EventSubscription;
using domainD.EventSubscription.NEventStore;
using Microsoft.Extensions.Hosting;
using Order.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace NEventStore.EventSubscription
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "Event Subscriptions";

            await new HostBuilder()
                .AddConfigFile()
                .AddEventSubscription<NEventStoreEventSubscription>(cfg => cfg
                    .UseFileCheckpointLoader()
                    .On<ProductAddedToOrder>().HandleAsync(HandlerFor.ProductAddedToOrder)
                    .On<ProductQuantityChanged>().HandleAsync(HandlerFor.ProductQuantityChanged)
                    .On<OrderCancelled>().HandleAsync(HandlerFor.OrderCancelled)
                    .On<OrderCompleted>().HandleAsync(HandlerFor.OrderCompleted)
                    .On<OrderPlaced>().HandleAsync(HandlerFor.OrderPlaced)
                    .On<ProductRemoved>().HandleAsync(HandlerFor.ProductRemoved)
                    .On<Delivery.Events.OrderPlaced>().HandleAsync(HandlerFor.DeliveryCreated)
                    .On<Delivery.Events.DeliveryStarted>().HandleAsync(HandlerFor.DeliveryStarted)
                    .On<Delivery.Events.OrderDelivered> ().HandleAsync(HandlerFor.OrderDelivered)
                    .On<Delivery.Events.OrderReturned> ().HandleAsync(HandlerFor.OrderReturned))
                .ConfigureServices(ServicesConfigurator.ConfigureBus)
                .ConfigureServices(ServicesConfigurator.ConfigureEventStore)
                .ConfigureServices(ServicesConfigurator.ConfigureEventSubscriptionServices)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}
