using System.Threading.Tasks;
using Delivery.Commands;
using domainD;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class StartDeliveryCommandHandler : ICommandHandler<StartDelivery>
    {
        private readonly IRepository<Domain.Order> _repository;

        public StartDeliveryCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(StartDelivery command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.StartDelivery();
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
