using domainD;
using MassTransit;
using Order.Commands;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandConsumers
{
    public class CompleteOrderConsumer : IConsumer<CompleteOrder>
    {
        private readonly ICommandHandler<CompleteOrder> _handler;

        public CompleteOrderConsumer(ICommandHandler<CompleteOrder> handler)
        {
            _handler = handler;
        }

        public  Task Consume(ConsumeContext<CompleteOrder> context)
        {
            return _handler.HandleAsync(context.Message);
        }
    }
}
