using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class CompleteCommandHandler : ICommandHandler<CompleteOrder>
    {
        private readonly IRepository<Domain.Order> _repository;

        public CompleteCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(CompleteOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Complete();
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
