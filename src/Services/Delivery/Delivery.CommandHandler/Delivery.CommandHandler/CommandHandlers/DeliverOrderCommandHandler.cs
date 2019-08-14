using System.Threading.Tasks;
using Delivery.Commands;
using domainD;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class DeliverOrderCommandHandler : ICommandHandler<DeliverOrder>
    {
        private readonly IRepository<Domain.Order> _repository;

        public DeliverOrderCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(DeliverOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Deliver();
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
