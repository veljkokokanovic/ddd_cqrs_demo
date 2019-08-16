using System.Threading.Tasks;
using Delivery.Commands;
using domainD;
using Microsoft.Extensions.Logging;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class DeliverOrderCommandHandler : ICommandHandler<DeliverOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public DeliverOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<DeliverOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(DeliverOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Deliver();
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for delivery {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
