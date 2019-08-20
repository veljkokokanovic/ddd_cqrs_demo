using MassTransit;
using MassTransit.Extensions;
using Microsoft.AspNetCore.SignalR;
using Order.Events;
using System;
using System.Threading.Tasks;
using UI.SignalR;

namespace UI.MessageConsumers
{
    public class ProductAddedToOrderConsumer : IConsumer<ProductAddedToOrder>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public ProductAddedToOrderConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<ProductAddedToOrder> context)
        {
            if(context.Headers.TryGetHeader(KnownHeaders.CorrelationId, out var correlationId) &&
               context.Headers.TryGetHeader(KnownHeaders.UserId, out var userId))
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync(
                    "CommandMessage",
                    new CommandMessage
                    {
                        CorrelationId = Guid.Parse(correlationId.ToString())
                    });
            }
        }
    }
}
