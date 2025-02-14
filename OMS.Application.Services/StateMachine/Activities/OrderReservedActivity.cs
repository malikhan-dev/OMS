using Automatonymous;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.Events;
using OMS.Application.Services.Jobs;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderReservedActivity : Activity<OrderStateInstance>
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
            throw new NotImplementedException();
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
            

                var model = _orderQueryRepository.GetById(context.Data.CorrelationId);

                model.Reserved();

                _orderCommandRepository.Update(model);

                await next.Execute(context).ConfigureAwait(false);

            



        }

        public async Task Execute(BehaviorContext<OrderStateInstance> context, Behavior<OrderStateInstance> next)
        {
         
             

                var model = _orderQueryRepository.GetById(context.Instance.CorrelationId);

                model.Reserved();

                _orderCommandRepository.Update(model);

                await next.Execute(context).ConfigureAwait(false);

            
        }

        public async Task Execute<T>(BehaviorContext<OrderStateInstance, T> context, Behavior<OrderStateInstance, T> next)
        {
           

                var model = _orderQueryRepository.GetById(context.Instance.CorrelationId);

                model.Reserved();

                _orderCommandRepository.Update(model);

                await next.Execute(context).ConfigureAwait(false);

            
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReserved, TException> context, Behavior<OrderStateInstance, StockReserved> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, TException> context, Behavior<OrderStateInstance> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public Task Faulted<T, TException>(BehaviorExceptionContext<OrderStateInstance, T, TException> context, Behavior<OrderStateInstance, T> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("publish-order-closed");
        }
    }

}
