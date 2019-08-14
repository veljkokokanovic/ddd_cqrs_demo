using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class CancelOrderCommandHandler : ICommandHandler<CancelOrder>
    {
        private readonly IRepository<Domain.Order> _repository;

        public CancelOrderCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(CancelOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Cancel();
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
