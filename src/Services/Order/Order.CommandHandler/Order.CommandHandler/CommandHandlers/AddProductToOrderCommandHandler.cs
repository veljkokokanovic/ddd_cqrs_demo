using domainD;
using Microsoft.Extensions.Logging;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public AddProductToOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<AddProductToOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(AddProductToOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            if (order != null)
            {
                order.AddProduct(command.Sku, command.Quantity, command.Price);
            }
            else
            {
                order = AggregateRoot.Create<Domain.Order>(new ProductAddedToOrder(command.Sku, command.Quantity, command.Price, command.UserId),
                    command.OrderId);
            }

            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
