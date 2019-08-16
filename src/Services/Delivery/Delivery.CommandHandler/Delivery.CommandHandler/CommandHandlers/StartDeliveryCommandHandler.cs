using System.Threading.Tasks;
using Delivery.Commands;
using domainD;
using Microsoft.Extensions.Logging;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class StartDeliveryCommandHandler : ICommandHandler<StartDelivery>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public StartDeliveryCommandHandler(IRepository<Domain.Order> repository, ILogger<StartDeliveryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(StartDelivery command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.StartDelivery();
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
