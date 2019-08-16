using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Extensions
{
    public static class KnownHeaders
    {
        public const string CorrelationId = nameof(CorrelationId);

        public const string UserId = nameof(UserId);
    }
}
