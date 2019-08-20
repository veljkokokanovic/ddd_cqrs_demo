using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Extensions
{
    public class CommandFailed
    {
        public string Reason { get; set; }

        public Guid CorrelationId { get; set; }

        public string CommandName { get; set; }

        public Guid UserId { get; set; }
    }
}
