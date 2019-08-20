using domainD;
using GreenPipes;
using System.Threading.Tasks;

namespace MassTransit.Extensions.Filters
{
    public class OperationContextEstablisher : IFilter<SendContext>
    {
        public Task Send(SendContext context, IPipe<SendContext> next)
        {
            context.Headers.Set(KnownHeaders.CorrelationId, OperationContext.CorrelationId);
            context.Headers.Set(KnownHeaders.UserId, OperationContext.UserId);

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope(nameof(OperationContextEstablisher));
        }
    }
}
