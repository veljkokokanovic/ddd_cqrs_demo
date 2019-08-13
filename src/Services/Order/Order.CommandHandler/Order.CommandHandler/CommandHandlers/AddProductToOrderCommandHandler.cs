using domainD;
using Order.Commands;
using Order.Events;
using System.Threading.Tasks;
using domainD.Repository;

namespace Order.CommandHandler.CommandHandlers
{
    public class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrder>
    {
        private IRepository<Domain.Order> _repository;

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
                order = AggregateRoot.Create<Domain.Order>(new ProductAddedToOrder
                        {Quantity = command.Quantity, Sku = command.Sku, UserId = OperationContext.UserId.Value},
                    command.OrderId);
            }

            await _repository.SaveAsync(order).ConfigureAwait(false);
        }
    }
}
