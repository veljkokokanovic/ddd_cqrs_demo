using Delivery.Commands;
using domainD;
using MassTransit;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class DeliverOrderConsumer : IConsumer<DeliverOrder>
    {
        private readonly ICommandHandler<DeliverOrder> _handler;

        public DeliverOrderConsumer(ICommandHandler<DeliverOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<DeliverOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
