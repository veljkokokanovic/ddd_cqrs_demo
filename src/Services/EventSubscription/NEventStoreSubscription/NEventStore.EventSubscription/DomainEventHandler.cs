using domainD;
using MassTransit;
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

        private DomainEventHandler(IReadModelRepository<TEntity, Guid> repository, DomainEvent orderEvent, IBus bus)
        {
            _repository = repository;
            _event = orderEvent;
            _bus = bus;
        }

        public static DomainEventHandler<TEntity> For(IReadModelRepository<TEntity, Guid> repository, DomainEvent @event, IBus bus = null)
        {
            return new DomainEventHandler<TEntity>(repository, @event, bus);
        }

        public async Task Manage(Action<TEntity> eventHandler, Func<TEntity> initialEventHandler = null)
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
                if (_bus != null)
                {
                    await _bus.Publish(_event, _event.GetType()).ConfigureAwait(false);
                }
                transaction.Complete();
            }
        }
    }
}
