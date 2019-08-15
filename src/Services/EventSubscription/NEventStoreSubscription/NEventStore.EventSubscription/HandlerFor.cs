using AutoMapper;
using MassTransit;
using Order.Events;
using ReadModel.Order;
using ReadModel.Repository.MsSql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NEventStore.EventSubscription
{
    public static class HandlerFor
    {
        public static Func<ProductAddedToOrder, IOrderRepository, IMapper, IBus, Task>
            ProductAddedToOrder =
                async (@event, repository, mapper, bus) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus)
                        .Manage(
                            order => order.Products.Add(mapper.Map<OrderItem>(@event)),
                            () => mapper.Map<ReadModel.Order.Order>(@event));
                };

        public static Func<ProductQuantityChanged, IOrderRepository, IBus, Task>
            ProductQuantityChanged =
                async (@event, repository, bus) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus)
                        .Manage(o =>
                        {
                            var product = o.Products.FirstOrDefault(p => p.Id.Sku == @event.Sku);
                            if (product != null)
                            {
                                product.Quantity = @event.To;
                            }
                        });
                };

        public static Func<OrderCancelled, IOrderRepository, IBus, Task>
            OrderCancelled =
                async (@event, repository, bus) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus).Manage(o =>
                        {
                            o.Status = OrderStatus.Cancelled;
                        });
                };

        public static Func<OrderCompleted, IOrderRepository, IBus, Task>
            OrderCompleted =
                async (@event, repository, bus) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus).Manage(o =>
                    {
                        o.Status = OrderStatus.Completed;
                    });
                };

        public static Func<OrderPlaced, IOrderRepository, IBus, Task>
            OrderPlaced =
                async (@event, repository, bus) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus).Manage(o =>
                    {
                        o.Status = OrderStatus.Placed;
                    });
                };

        public static Func<ProductRemoved, IOrderRepository, IBus, Task>
            ProductRemoved =
                async (@event, repository, bus) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus).Manage(o =>
                    {
                        var product = o.Products.FirstOrDefault(p => p.Id.Sku == @event.Sku);
                        if (product != null)
                        {
                            o.Products.Remove(product);
                        }
                    });
                };
    }
}
