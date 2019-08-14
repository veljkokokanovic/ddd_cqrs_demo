using domainD;
using MassTransit;
using Order.Commands;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class RemoveProductConsumer : IConsumer<RemoveProduct>
    {
        private readonly ICommandHandler<RemoveProduct> _handler;

        public RemoveProductConsumer(ICommandHandler<RemoveProduct> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<RemoveProduct> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
