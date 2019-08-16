using System;
using System.Linq;
using System.Threading.Tasks;
using Delivery.Commands;
using Delivery.Events;
using domainD;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public PlaceOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<PlaceOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(PlaceOrder command)
        {
            var addressParts = command.Address.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var address = new Address(addressParts.ElementAt(0), addressParts.ElementAt(1), addressParts.ElementAt(2));
            var delivery = new SharedKernel.Delivery(address, command.DeliveryDate, command.PhoneNumber);
            var orderPlacedEvent = new OrderPlaced(delivery, command.UserId, command.ReferenceOrderId);

            var order = AggregateRoot.Create<Domain.Order>(orderPlacedEvent);
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.ReferenceOrderId);
        }
    }
}
