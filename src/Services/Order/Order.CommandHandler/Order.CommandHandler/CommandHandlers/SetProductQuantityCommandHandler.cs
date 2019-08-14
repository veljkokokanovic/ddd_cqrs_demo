using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class SetProductQuantityCommandHandler : ICommandHandler<SetProductQuantity>
    {
        private readonly IRepository<Domain.Order> _repository;

        public SetProductQuantityCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(SetProductQuantity command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            if (order.TryGetOrderItem(command.Sku, out var item))
            {
                item.SetQuantity(command.Quantity);
                await _repository.SaveAsync(order).ConfigureAwait(false);
            }
        }
    }
}
