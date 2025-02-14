using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Application.Services.Events.Services;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    internal class OrderPaidActivity : Activity<OrderStateInstance, SuccessfullyPaidEvent>
    {
        private readonly IOrderCommandRepository _orderCommandRepository;
        private readonly IOrderQueryRepository _orderQueryRepository;
        private readonly EventStorage eventStoreService;

        public OrderPaidActivity(IOrderCommandRepository orderCommandRepository, IOrderQueryRepository orderQueryRepository, EventStorage eventStoreService)
        {
            _orderCommandRepository = orderCommandRepository;
            _orderQueryRepository = orderQueryRepository;
            this.eventStoreService = eventStoreService;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, SuccessfullyPaidEvent> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next)
        {

            await eventStoreService.AppendEvents($"Order:{context.Data.CorrelationId} Paid", nameof(OrderPaidActivity));


            var model = _orderQueryRepository.GetById(context.Instance.CorrelationId);

            model.Paid();

            _orderCommandRepository.Update(model);

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, SuccessfullyPaidEvent, TException> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }
        public void Probe(ProbeContext context)
        {
            context.CreateScope("publish-order-closed");
        }
    }
}
