using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderFailedActivity : Activity<OrderStateInstance, StockReservationFailed>, Activity<OrderStateInstance, PaymentFailedEvent>
    {

        private  IOrderQueryRepository _orderQueryRepository;

        private  IOrderCommandRepository _orderRepository;

        public OrderFailedActivity(IOrderCommandRepository orderRepository, IOrderQueryRepository orderQueryRepository)
        {
            _orderRepository = orderRepository;

            _orderQueryRepository = orderQueryRepository;
        }

 
        public async Task Execute(BehaviorContext<OrderStateInstance, StockReservationFailed> context, Behavior<OrderStateInstance, StockReservationFailed> next)
        {
            await Failed(context.Data.CorrelationId);
            //await next.Execute(context).ConfigureAwait(false);
        }

        private Task Failed(Guid OrderId)
        {
            var model = this._orderQueryRepository.GetById(OrderId);

            model.Failed();

            _orderRepository.Update(model);

            return Task.CompletedTask;
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

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReservationFailed, TException> context, Behavior<OrderStateInstance, StockReservationFailed> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, PaymentFailedEvent> context, Behavior<OrderStateInstance, PaymentFailedEvent> next)
        {
            await Failed(context.Data.CorrelationId);
            //await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, PaymentFailedEvent, TException> context, Behavior<OrderStateInstance, PaymentFailedEvent> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }

}
