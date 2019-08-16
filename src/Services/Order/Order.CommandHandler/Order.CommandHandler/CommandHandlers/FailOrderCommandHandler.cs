using domainD;
using Order.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class FailOrderCommandHandler : ICommandHandler<FailOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public FailOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<FailOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(FailOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Fail(command.Reason);
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
