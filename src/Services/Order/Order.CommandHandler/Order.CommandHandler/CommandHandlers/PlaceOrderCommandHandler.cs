using System;
using System.Linq;
using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;
using SharedKernel;

namespace Order.CommandHandler.CommandHandlers
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
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            var addressParts = command.Address.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var address = new Address(addressParts.ElementAt(0), addressParts.ElementAt(1), addressParts.ElementAt(2));
            var delivery = new Delivery(address, command.DeliveryDate, command.PhoneNumber);
            order.Place(delivery);
            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
