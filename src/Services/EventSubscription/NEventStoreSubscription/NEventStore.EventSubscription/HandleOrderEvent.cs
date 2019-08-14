﻿using AutoMapper;
using domainD;
using Order.Events;
using ReadModel.Order;
using ReadModel.Repository.MsSql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MassTransit;

namespace NEventStore.EventSubscription
{
    public static class HandleOrderEvent
    {
        public static Func<ProductAddedToOrder, IOrderRepository, IMapper, IBus, Task>
            ProductAddedToOrder =
                async (@event, repository, mapper, bus) =>
                {
                    await OrderEvent.For(repository, @event, bus).Manage(
                        order => order.Products.Add(mapper.Map<OrderItem>(@event)),
                        () => mapper.Map<ReadModel.Order.Order>(@event));
                };

        public static Func<ProductQuantityChanged, IOrderRepository, IBus, Task>
            ProductQuantityChanged =
                async (@event, repository, bus) =>
                {
                    await OrderEvent.For(repository, @event, bus).Manage(o =>
                    {
                        var lineItem = o.Products.FirstOrDefault(p => p.Id.Sku == @event.Sku);
                        if (lineItem != null)
                        {
                            lineItem.Quantity = @event.To;
                        }
                    });
                };
    }

    internal class OrderEvent
    {
        private IOrderRepository _repository;
        private DomainEvent _event;
        private IBus _bus;

        private OrderEvent(IOrderRepository repository, DomainEvent orderEvent, IBus bus)
        {
            _repository = repository;
            _event = orderEvent;
            _bus = bus;
        }

        public static OrderEvent For(IOrderRepository repository, DomainEvent @event, IBus bus = null)
        {
            return new OrderEvent(repository, @event, bus);
        }

        public async Task Manage(Action<ReadModel.Order.Order> eventHandler, Func<ReadModel.Order.Order> initialEventHandler = null)
        {
            var order = await _repository.GetAsync(_event.AggregateRootId).ConfigureAwait(false);
            if (order == null)
            {
                if (_event.Version == 0 && initialEventHandler != null)
                {
                    await SaveAndPublish(initialEventHandler);
                }
                else
                {
                    throw new InvalidOperationException($"Initial domain event {_event.GetType()} version should be 0, but was {_event.Version}");
                }
            }
            else
            {
                if (order.Version < _event.Version)
                {
                    await SaveAndPublish(() =>
                    {
                        eventHandler(order);
                        return order;
                    });
                }
            }
        }

        private async Task SaveAndPublish(Func<ReadModel.Order.Order> updateOrderModel)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var order = updateOrderModel();
                order.Version = _event.Version;
                await _repository.SaveAsync(order).ConfigureAwait(false);
                if (_bus != null)
                {
                    await _bus.Publish(_event, _event.GetType()).ConfigureAwait(false);
                }
                transaction.Complete();
            }
        }
    }
}