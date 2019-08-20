using MassTransit;
using MassTransit.Extensions;
using Microsoft.AspNetCore.SignalR;
using Order.Events;
using System;
using System.Threading.Tasks;
using UI.SignalR;

namespace UI.MessageConsumers
{
    public class CommandErrorConsumer : IConsumer<CommandFailed>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommandErrorConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<CommandFailed> context)
        {
            await _hubContext.Clients.User(context.Message.UserId.ToString()).SendAsync(
                    "CommandMessage",
                    new CommandMessage
                    {
                        Success = false,
                        CorrelationId = context.Message.CorrelationId,
                        Description = context.Message.Reason
                    });
        }
    }
}
