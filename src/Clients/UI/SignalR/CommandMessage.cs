using System;

namespace UI.SignalR
{
    public class CommandMessage
    {
        public bool Success { get; set; } = true;

        public Guid CorrelationId { get; set; }
    }
}