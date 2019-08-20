using domainD;
using GreenPipes;
using System;
using System.Threading.Tasks;

namespace MassTransit.Extensions.Filters
{
    public class OperationContextFilter : IFilter<ConsumeContext>
    {
        public Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            if (context.Headers.TryGetHeader(KnownHeaders.CorrelationId, out var correlationId) && Guid.TryParse(correlationId.ToString(), out var correlationGuid))
            {
                OperationContext.CorrelationId = correlationGuid;
            }

            if (context.Headers.TryGetHeader(KnownHeaders.UserId, out var userId) && Guid.TryParse(userId.ToString(), out var userIdGuid))
            {
                OperationContext.UserId = userIdGuid;
            }

            OperationContext.CommandId = context.MessageId;

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope(nameof(OperationContextFilter));
        }
    }
}
