using System.Threading.Tasks;
using Delivery.Commands;
using domainD;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class ReturnOrderCommandHandler : ICommandHandler<ReturnOrder>
    {
        private readonly IRepository<Domain.Order> _repository;

        public ReturnOrderCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(ReturnOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Return(command.Reason);
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
