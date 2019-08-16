using System.Threading.Tasks;
using Delivery.Commands;
using domainD;
using Microsoft.Extensions.Logging;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class ReturnOrderCommandHandler : ICommandHandler<ReturnOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public ReturnOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<ReturnOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(ReturnOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Return(command.Reason);
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
