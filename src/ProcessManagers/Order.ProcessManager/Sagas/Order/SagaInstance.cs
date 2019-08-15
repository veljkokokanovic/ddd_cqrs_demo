using System;
using System.Collections.Generic;
using System.Text;
using Automatonymous;

namespace Order.ProcessManager.Sagas.Order
{
    public class SagaInstance : SagaStateMachineInstance
    {
        public SagaInstance(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        protected SagaInstance()
        {
        }

        public string CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
