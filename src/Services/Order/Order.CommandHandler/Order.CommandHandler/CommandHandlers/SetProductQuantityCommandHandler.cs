using domainD;
using Order.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class SetProductQuantityCommandHandler : ICommandHandler<SetProductQuantity>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public SetProductQuantityCommandHandler(IRepository<Domain.Order> repository, ILogger<SetProductQuantityCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(SetProductQuantity command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            if (order.TryGetOrderItem(command.Sku, out var item))
            {
                item.SetQuantity(command.Quantity);
                await _repository.SaveAsync(order).ConfigureAwait(false);
            }
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
