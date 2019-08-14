using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class RemoveProductCommandHandler : ICommandHandler<RemoveProduct>
    {
        private readonly IRepository<Domain.Order> _repository;

        public RemoveProductCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(RemoveProduct command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            if (order.TryGetOrderItem(command.Sku, out var item))
            {
                item.Remove();
                await _repository.SaveAsync(order).ConfigureAwait(false);
            }
        }
    }
}
