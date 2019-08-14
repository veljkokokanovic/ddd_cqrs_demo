using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrder>
    {
        private readonly IRepository<Domain.Order> _repository;

        public AddProductToOrderCommandHandler(IRepository<Domain.Order> repository)
        {
            _repository = repository;
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
        }
    }
}
