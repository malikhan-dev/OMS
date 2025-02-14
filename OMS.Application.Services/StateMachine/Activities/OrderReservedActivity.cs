using Automatonymous;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.Events;
using OMS.Application.Services.Jobs;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderReservedActivity :  Activity<OrderStateInstance, StockReserved>
    {

        private readonly IOrderCommandRepository _orderCommandRepository;
        private readonly IOrderQueryRepository _orderQueryRepository;
        public OrderReservedActivity(IOrderCommandRepository orderCommandRepository, IOrderQueryRepository orderQueryRepository)
        {
            _orderCommandRepository = orderCommandRepository;
            _orderQueryRepository = orderQueryRepository;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
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
