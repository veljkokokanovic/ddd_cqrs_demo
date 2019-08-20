using domainD;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Extensions
{
    public class CommandErrorObserver : IConsumeObserver
    {
        public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            if(typeof(ICommand).IsAssignableFrom(typeof(T)) 
                && context.Headers.TryGetHeader(KnownHeaders.CorrelationId, out var correlationId)
                && context.Headers.TryGetHeader(KnownHeaders.UserId, out var userId))
            {
                await context.Publish(new CommandFailed
                {
                    CommandName = typeof(T).Name,
                    CorrelationId = Guid.Parse(correlationId.ToString()),
                    Reason = exception.Message,
                    UserId = Guid.Parse(userId.ToString())
                });
            }
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
