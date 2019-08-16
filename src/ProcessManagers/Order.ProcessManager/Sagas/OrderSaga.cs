using Automatonymous;
using Automatonymous.Activities;
using Automatonymous.Binders;
using Delivery.Events;
using Microsoft.Extensions.Logging;
using Order.ProcessManager.Sagas.Persistence;
using System;
using System.Threading.Tasks;
using OrderPlaced = Order.Events.OrderPlaced;

namespace Order.ProcessManager.Sagas
{
    public class OrderSaga : MassTransitStateMachine<SagaInstance>
    {
        private readonly ILogger _logger;

        public OrderSaga(ILogger<OrderSaga> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);
            SetCompletedWhenFinalized();

            Event(() => OrderPlacedEvent, c => c.CorrelateById(ctx => ctx.Message.AggregateRootId));
            Event(() => OrderDeliveredEvent, c => c.CorrelateById(ctx => ctx.Message.ReferenceOrderId));
            Event(() => OrderReturnedEvent, c => c.CorrelateById(ctx => ctx.Message.ReferenceOrderId));

            Initially(
                When(OrderPlacedEvent)
                    .Then(InitializeSaga)
                    .ThenAsync(CreateDelivery)
                    .TransitionTo(Placed));

            During(Placed,
                When(OrderDeliveredEvent)
                .ThenAsync(MarkOrderAsCompleted)
                .TransitionTo(Delivered)
                .Finalize());

            During(Placed,
                When(OrderReturnedEvent)
                .ThenAsync(MarkOrderAsFailed)
                .TransitionTo(Returned)
                .Finalize());

            WhenEnterAny(LogStateTransition);
        }

        private EventActivityBinder<SagaInstance> LogStateTransition(EventActivityBinder<SagaInstance> binder)
        {
            return binder.Add(new ActionActivity<SagaInstance>(context =>
            {
                _logger.LogInformation(
                    "Order {order} workflow moved to {SagaState}.",
                    context.Instance.CorrelationId, context.Instance.CurrentState);
            }));
        }

        private async Task CreateDelivery(BehaviorContext<SagaInstance, OrderPlaced> context)
        {
            await context.Send(new Delivery.Commands.PlaceOrder
            {
                ReferenceOrderId = context.Data.AggregateRootId,
                UserId = context.Data.UserId,
                DeliveryDate = context.Data.DeliveryInfo.DeliveryDate,
                PhoneNumber = context.Data.DeliveryInfo.PhoneNumber,
                Address = string.Join(Environment.NewLine, context.Data.DeliveryInfo.DeliveryAddress.Line1, context.Data.DeliveryInfo.DeliveryAddress.Line2, context.Data.DeliveryInfo.DeliveryAddress.PostCode)
            });

            _logger.LogInformation("Sending {command} for {order}", nameof(Commands.PlaceOrder), context.Data.AggregateRootId);
        }

        private async Task MarkOrderAsFailed(BehaviorContext<SagaInstance, OrderReturned> context)
        {
            await context.Send(new Commands.FailOrder { OrderId = context.Data.ReferenceOrderId, Reason = "Returned, pizza cold" }).ConfigureAwait(false);
            _logger.LogInformation("Sending {command} for {order}", nameof(Commands.FailOrder), context.Data.ReferenceOrderId);
        }

        private async Task MarkOrderAsCompleted(BehaviorContext<SagaInstance, OrderDelivered> context)
        {
            await context.Send(new Commands.CompleteOrder { OrderId = context.Data.ReferenceOrderId });
            _logger.LogInformation("Sending {command} for {order}", nameof(Commands.CompleteOrder), context.Data.ReferenceOrderId);
        }

        private void InitializeSaga(BehaviorContext<SagaInstance, OrderPlaced> context)
        {
            context.Instance.CorrelationId = context.Data.AggregateRootId;
        }

        public Event<OrderPlaced> OrderPlacedEvent { get; set; }

        public Event<OrderDelivered> OrderDeliveredEvent { get; set; }

        public Event<OrderReturned> OrderReturnedEvent { get; set; }

        public State Placed { get; private set; }

        public State Returned { get; private set; }

        public State Delivered { get; private set; }
    }
}
