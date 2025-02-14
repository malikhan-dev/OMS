using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Application.Services.Events.Services;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderFailedByPaymentActivity : Activity<OrderStateInstance, PaymentFailedEvent>
    {

        private IOrderQueryRepository _orderQueryRepository;

        private IOrderCommandRepository _orderRepository;

        private readonly EventStorage eventStoreService;


        public OrderFailedByPaymentActivity(IOrderCommandRepository orderRepository, IOrderQueryRepository orderQueryRepository, EventStorage eventStoreService)
        {
            _orderRepository = orderRepository;

            _orderQueryRepository = orderQueryRepository;

            this.eventStoreService = eventStoreService;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, PaymentFailedEvent> context, Behavior<OrderStateInstance, PaymentFailedEvent> next)
        {
            await eventStoreService.AppendEvents($"Order:{context.Data.CorrelationId} Failed",nameof(OrderFailedByPaymentActivity));

            await Failed(context.Data.CorrelationId);

            await next.Execute(context).ConfigureAwait(false);
        }

        private Task Failed(Guid OrderId)
        {
            var model = this._orderQueryRepository.GetById(OrderId);

            model.Failed();

            _orderRepository.Update(model);

            return Task.CompletedTask;
        }
        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, PaymentFailedEvent, TException> context, Behavior<OrderStateInstance, PaymentFailedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("publish-order-closed");
        }
    }

}
