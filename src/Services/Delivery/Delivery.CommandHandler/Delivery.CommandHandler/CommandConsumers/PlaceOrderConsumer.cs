using Delivery.Commands;
using domainD;
using MassTransit;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class PlaceOrderConsumer : IConsumer<PlaceOrder>
    {
        private readonly ICommandHandler<PlaceOrder> _handler;

        public PlaceOrderConsumer(ICommandHandler<PlaceOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<PlaceOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
