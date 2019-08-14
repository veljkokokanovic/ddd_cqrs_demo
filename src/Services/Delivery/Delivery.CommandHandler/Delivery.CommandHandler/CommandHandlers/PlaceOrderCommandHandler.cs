using System;
using System.Linq;
using System.Threading.Tasks;
using Delivery.Commands;
using Delivery.Events;
using domainD;
using SharedKernel;

namespace Delivery.CommandHandler.CommandHandlers
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrder>
    {
        private readonly IRepository<Domain.Order> _repository;

        public PlaceOrderCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(PlaceOrder command)
        {
            var addressParts = command.Address.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var address = new Address(addressParts.ElementAt(0), addressParts.ElementAt(1), addressParts.ElementAt(2));
            var delivery = new SharedKernel.Delivery(address, command.DeliveryDate, command.PhoneNumber);
            var orderPlacedEvent = new OrderPlaced(delivery, command.UserId);

            var order = AggregateRoot.Create<Domain.Order>(orderPlacedEvent, command.OrderId);
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
