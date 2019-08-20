using domainD;
using GreenPipes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassTransit.Extensions.Filters
{
    public class PublishContextEstablisher : IPipeSpecification<SendContext>, IFilter<SendContext>
    {
        void IPipeSpecification<SendContext>.Apply(IPipeBuilder<SendContext> builder)
        {
            builder.AddFilter(this);
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            context.CreateFilterScope(nameof(PublishContextEstablisher));
        }

        Task IFilter<SendContext>.Send(SendContext context, IPipe<SendContext> next)
        {
            context.Headers.Set(KnownHeaders.CorrelationId, OperationContext.CorrelationId);
            context.Headers.Set(KnownHeaders.UserId, OperationContext.UserId);
            return next.Send(context);
        }

        IEnumerable<ValidationResult> ISpecification.Validate()
        {
            yield break;
        }
    }
}
