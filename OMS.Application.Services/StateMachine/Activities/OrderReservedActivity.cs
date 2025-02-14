using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Application.Services.Events.Services;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderReservedActivity :  Activity<OrderStateInstance, StockReserved>
    {

        private readonly IOrderCommandRepository _orderCommandRepository;
        private readonly IOrderQueryRepository _orderQueryRepository;
        private readonly EventStorage eventStoreService;

        public OrderReservedActivity(IOrderCommandRepository orderCommandRepository, IOrderQueryRepository orderQueryRepository, EventStorage eventStoreService)
        {
            _orderCommandRepository = orderCommandRepository;
            _orderQueryRepository = orderQueryRepository;
            this.eventStoreService = eventStoreService;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
            await eventStoreService.AppendEvents($"Order:{context.Data.CorrelationId} Paid", nameof(OrderReservedActivity));


            var model = _orderQueryRepository.GetById(context.Data.CorrelationId);

            model.Reserved();

            _orderCommandRepository.Update(model);

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReserved, TException> context, Behavior<OrderStateInstance, StockReserved> next) where TException : Exception
        {
            return next.Faulted(context);
        }
        public void Probe(ProbeContext context)
        {
            context.CreateScope("publish-order-closed");
        }
    }

}
