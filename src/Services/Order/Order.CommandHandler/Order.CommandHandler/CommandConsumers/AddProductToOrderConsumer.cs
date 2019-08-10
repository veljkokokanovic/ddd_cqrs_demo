using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using domainD;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Commands;

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
