using domainD;
using MassTransit;
using Order.Commands;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class SetProductQuantityConsumer : IConsumer<SetProductQuantity>
    {
        private readonly ICommandHandler<SetProductQuantity> _handler;

        public SetProductQuantityConsumer(ICommandHandler<SetProductQuantity> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<SetProductQuantity> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
