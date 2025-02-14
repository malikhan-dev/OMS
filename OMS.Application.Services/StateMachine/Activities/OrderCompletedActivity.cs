using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Application.Services.Events.Services;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderCompletedActivity : Activity<OrderStateInstance, StockReserved>, Activity<OrderStateInstance, SuccessfullyPaidEvent>
    {
        private readonly IOrderQueryRepository _orderQueryRepository;

        private readonly IOrderCommandRepository _orderCommandRepository;

        private readonly EventStorage eventStoreService;

        public OrderCompletedActivity(IOrderQueryRepository orderQueryRepository, IOrderCommandRepository orderCommandRepository, EventStorage eventStoreService)
        {
            _orderQueryRepository = orderQueryRepository;
            _orderCommandRepository = orderCommandRepository;
            this.eventStoreService = eventStoreService;
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
            await eventStoreService.AppendEvents($"Order:{context.Data.CorrelationId} Completed", nameof(OrderCompletedActivity));

            CompleteOrder(context.Data.CorrelationId);

            await next.Execute(context).ConfigureAwait(false);
        }

        private void CompleteOrder(Guid orderId)
        {
            var model = _orderQueryRepository.GetById(orderId);

            model.Completed();

            _orderCommandRepository.Update(model);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, SuccessfullyPaidEvent> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next)
        {
            await eventStoreService.AppendEvents($"Order:{context.Data.CorrelationId} Completed", nameof(OrderCompletedActivity));

            CompleteOrder(context.Data.OrderId);

            await next.Execute(context).ConfigureAwait(false);

        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReserved, TException> context, Behavior<OrderStateInstance, StockReserved> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, SuccessfullyPaidEvent, TException> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("publish-order-closed");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
