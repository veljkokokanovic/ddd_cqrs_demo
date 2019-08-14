using Delivery.Commands;
using domainD;
using MassTransit;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class ReturnOrderConsumer : IConsumer<ReturnOrder>
    {
        private readonly ICommandHandler<ReturnOrder> _handler;

        public ReturnOrderConsumer(ICommandHandler<ReturnOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<ReturnOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
