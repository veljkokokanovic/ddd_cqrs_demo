using domainD;
using MassTransit;
using Order.Commands;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class AddProductToOrderConsumer : IConsumer<AddProductToOrder>
    {
        private readonly ICommandHandler<AddProductToOrder> _handler;

        public AddProductToOrderConsumer(ICommandHandler<AddProductToOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<AddProductToOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
