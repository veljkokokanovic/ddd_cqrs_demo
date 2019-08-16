using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Events;
using ReadModel.Order;
using ReadModel.Repository.MsSql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NEventStore.EventSubscription
{
    internal static class HandlerFor
    {
        #region order events

        public static Func<ProductAddedToOrder, IOrderRepository, IMapper, IBus, ILogger<DomainEventHandler<ReadModel.Order.Order>>, Task>
            ProductAddedToOrder =
                async (@event, repository, mapper, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus, logger)
                        .Manage(
                            order => order.Products.Add(mapper.Map<OrderItem>(@event)),
                            () => mapper.Map<ReadModel.Order.Order>(@event));
                };

        public static Func<ProductQuantityChanged, IOrderRepository, IBus, ILogger<DomainEventHandler<ReadModel.Order.Order>>, Task>
            ProductQuantityChanged =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus, logger)
                        .Manage(o =>
                        {
                            var product = o.Products.FirstOrDefault(p => p.Id.Sku == @event.Sku);
                            if (product != null)
                            {
                                product.Quantity = @event.To;
                            }
                        });
                };

        public static Func<OrderCancelled, IOrderRepository, IBus, ILogger<DomainEventHandler<ReadModel.Order.Order>>, Task>
            OrderCancelled =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus, logger).Manage(o =>
                        {
                            o.Status = OrderStatus.Cancelled;
                        });
                };

        public static Func<OrderCompleted, IOrderRepository, IBus, ILogger<DomainEventHandler<ReadModel.Order.Order>>, Task>
            OrderCompleted =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus, logger).Manage(o =>
                    {
                        o.Status = @event.Success ? OrderStatus.Completed : OrderStatus.Failed;
                        o.Comment = @event.Comment;
                    });
                };

        public static Func<OrderPlaced, IOrderRepository, IBus, ILogger<DomainEventHandler<ReadModel.Order.Order>>, Task>
            OrderPlaced =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus, logger).Manage(o =>
                    {
                        o.Status = OrderStatus.Placed;
                    });
                };

        public static Func<ProductRemoved, IOrderRepository, IBus, ILogger<DomainEventHandler<ReadModel.Order.Order>>, Task>
            ProductRemoved =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Order.Order>.For(repository, @event, bus, logger).Manage(o =>
                    {
                        var product = o.Products.FirstOrDefault(p => p.Id.Sku == @event.Sku);
                        if (product != null)
                        {
                            o.Products.Remove(product);
                        }
                    });
                };

        #endregion

        #region delivery events

        public static Func<Delivery.Events.OrderPlaced, IDeliveryRepository, IMapper, IBus, ILogger<DomainEventHandler<ReadModel.Delivery.Order>>, Task>
            DeliveryCreated =
                async (@event, repository, mapper, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Delivery.Order>.For(repository, @event, bus, logger)
                        .Manage(initialEventHandler:() =>
                        {
                            return mapper.Map<ReadModel.Delivery.Order>(@event);
                        });
                };

        public static Func<Delivery.Events.DeliveryStarted, IDeliveryRepository, IBus, ILogger<DomainEventHandler<ReadModel.Delivery.Order>>, Task>
            DeliveryStarted =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Delivery.Order>.For(repository, @event, bus, logger).Manage(d =>
                    {
                        d.Status = ReadModel.Delivery.DeliveryStatus.Delivering;
                        d.DeliveryStartedAt = @event.CreatedOn;
                    });
                };

        public static Func<Delivery.Events.OrderDelivered, IDeliveryRepository, IBus, ILogger<DomainEventHandler<ReadModel.Delivery.Order>>, Task>
            OrderDelivered =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Delivery.Order>.For(repository, @event, bus, logger).Manage(d =>
                    {
                        d.Status = ReadModel.Delivery.DeliveryStatus.Delivered;
                        d.DeliveredAt = @event.CreatedOn;
                    });
                };

        public static Func<Delivery.Events.OrderReturned, IDeliveryRepository, IBus, ILogger<DomainEventHandler<ReadModel.Delivery.Order>>, Task>
            OrderReturned =
                async (@event, repository, bus, logger) =>
                {
                    await DomainEventHandler<ReadModel.Delivery.Order>.For(repository, @event, bus, logger).Manage(d =>
                    {
                        d.Status = ReadModel.Delivery.DeliveryStatus.Returned;
                    });
                };

        #endregion
    }
}
