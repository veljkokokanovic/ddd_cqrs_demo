using domainD;
using MassTransit;
using Order.Commands;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class FailOrderConsumer : IConsumer<FailOrder>
    {
        private readonly ICommandHandler<FailOrder> _handler;

        public FailOrderConsumer(ICommandHandler<FailOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<FailOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
