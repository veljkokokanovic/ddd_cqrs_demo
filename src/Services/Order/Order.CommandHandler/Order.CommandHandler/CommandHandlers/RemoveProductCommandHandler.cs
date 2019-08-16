using domainD;
using Order.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class RemoveProductCommandHandler : ICommandHandler<RemoveProduct>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public RemoveProductCommandHandler(IRepository<Domain.Order> repository, ILogger<RemoveProductCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(RemoveProduct command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            if (order.TryGetOrderItem(command.Sku, out var item))
            {
                item.Remove();
                await _repository.SaveAsync(order).ConfigureAwait(false);
            }
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
