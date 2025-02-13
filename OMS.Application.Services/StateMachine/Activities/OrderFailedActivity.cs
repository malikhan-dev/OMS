using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderFailedActivity : Activity<OrderStateInstance, StockReservationFailed>, Activity<OrderStateInstance, PaymentFailedEvent>
    {

        private readonly IOrderQueryRepository _orderQueryRepository;

        private readonly IOrderCommandRepository _orderRepository;

        public OrderFailedActivity(IOrderCommandRepository orderRepository, IOrderQueryRepository orderQueryRepository)
        {
            _orderRepository = orderRepository;

            _orderQueryRepository = orderQueryRepository;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public async  Task Execute(BehaviorContext<OrderStateInstance, StockReservationFailed> context, Behavior<OrderStateInstance, StockReservationFailed> next)
        {
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

        public async Task Execute(BehaviorContext<OrderStateInstance, PaymentFailedEvent> context, Behavior<OrderStateInstance, PaymentFailedEvent> next)
        {
             await Failed(context.Data.CorrelationId);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReservationFailed, TException> context, Behavior<OrderStateInstance, StockReservationFailed> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, PaymentFailedEvent, TException> context, Behavior<OrderStateInstance, PaymentFailedEvent> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }
    }

}
