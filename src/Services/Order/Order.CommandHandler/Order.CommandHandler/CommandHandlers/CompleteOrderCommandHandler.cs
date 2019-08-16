using domainD;
using Order.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class CompleteOrderCommandHandler : ICommandHandler<CompleteOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public CompleteOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<CompleteOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(CompleteOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Complete();
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
