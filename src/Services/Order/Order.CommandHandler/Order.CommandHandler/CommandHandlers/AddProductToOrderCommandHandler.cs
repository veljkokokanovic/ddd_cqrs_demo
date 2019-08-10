using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using domainD;
using Order.Commands;

namespace Order.CommandHandler.CommandHandlers
{
    public class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrder>
    {
        public  Task HandleAsync(AddProductToOrder command)
        {
            return Task.CompletedTask;
        }
    }
}
