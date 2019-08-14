using domainD;
using MassTransit;
using Order.Commands;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class CancelOrderConsumer : IConsumer<CancelOrder>
    {
        private readonly ICommandHandler<CancelOrder> _handler;

        public CancelOrderConsumer(ICommandHandler<CancelOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<CancelOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
