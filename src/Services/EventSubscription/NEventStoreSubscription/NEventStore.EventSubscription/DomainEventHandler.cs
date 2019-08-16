using domainD;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ReadModel.Repository;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace NEventStore.EventSubscription
{
    internal class DomainEventHandler<TEntity>
        where TEntity : ReadModel.Entity<Guid>
    {
        private readonly IReadModelRepository<TEntity, Guid> _repository;
        private readonly DomainEvent _event;
        private readonly IBus _bus;
        private readonly ILogger _logger;

        private DomainEventHandler(IReadModelRepository<TEntity, Guid> repository, DomainEvent orderEvent, IBus bus, ILogger<DomainEventHandler<TEntity>> logger)
        {
            _repository = repository;
            _event = orderEvent;
            _bus = bus;
            _logger = logger ?? NullLogger<DomainEventHandler<TEntity>>.Instance;
        }

        public static DomainEventHandler<TEntity> For(IReadModelRepository<TEntity, Guid> repository, DomainEvent @event, IBus bus = null, ILogger<DomainEventHandler<TEntity>> logger = null)
        {
            return new DomainEventHandler<TEntity>(repository, @event, bus, logger);
        }

        public async Task Manage(Action<TEntity> eventHandler = null, Func<TEntity> initialEventHandler = null)
        {
            var entiy = await _repository.GetAsync(_event.AggregateRootId).ConfigureAwait(false);
            if (entiy == null)
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
                if (entiy.Version < _event.Version)
                {
                    await SaveAndPublish(() =>
                    {
                        eventHandler(entiy);
                        return entiy;
                    });
                }
            }
        }

        private async Task SaveAndPublish(Func<TEntity> updateModel)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = updateModel();
                entity.Version = _event.Version;
                await _repository.SaveAsync(entity).ConfigureAwait(false);
                _logger.LogInformation("Event {event} for order {order} succesfully projected to read store.", _event.GetType(), _event.AggregateRootId);
                if (_bus != null)
                {
                    await _bus.Publish(_event, _event.GetType()).ConfigureAwait(false);
                    _logger.LogInformation("Event {event} for order {order} succesfully published.", _event.GetType(), _event.AggregateRootId);
                }
                transaction.Complete();
            }
        }
    }
}
