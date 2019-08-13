using System;
using System.Threading.Tasks;
using domainD.Repository;
using GreenPipes;
using MassTransit;
using MassTransit.Extensions;

namespace Console.Host.Filters
{
    public class OperationContextFilter : IFilter<ConsumeContext>
    {
        public Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            if (context.Headers.TryGetHeader(KnownHeaders.CorrelationId, out var correlationId) && Guid.TryParse(correlationId.ToString(), out var correlationGuid))
            {
                OperationContext.CorrelationId = correlationGuid;
            }

            OperationContext.UserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope(nameof(OperationContextFilter));
        }
    }
}
