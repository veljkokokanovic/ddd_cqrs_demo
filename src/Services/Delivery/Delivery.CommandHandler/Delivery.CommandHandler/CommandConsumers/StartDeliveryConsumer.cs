using Delivery.Commands;
using domainD;
using MassTransit;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class StartDeliveryConsumer : IConsumer<StartDelivery>
    {
        private readonly ICommandHandler<StartDelivery> _handler;

        public StartDeliveryConsumer(ICommandHandler<StartDelivery> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<StartDelivery> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
