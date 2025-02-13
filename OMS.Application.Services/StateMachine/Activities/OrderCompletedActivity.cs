using Automatonymous;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.Events;
using OMS.Domain.Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderCompletedActivity : Activity<OrderStateInstance, StockReserved>, Activity<OrderStateInstance, SuccessfullyPaidEvent>
    {
        private readonly IOrderQueryRepository _orderQueryRepository;

        private readonly IOrderCommandRepository _orderCommandRepository;

        private readonly IServiceProvider _serviceProvider;
        public OrderCompletedActivity(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
            CompleteOrder(context.Data.CorrelationId);

            await next.Execute(context).ConfigureAwait(false);
        }

        private void CompleteOrder(Guid orderId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _orderCommandRepository =
                    scope.ServiceProvider
                        .GetRequiredService<IOrderCommandRepository>();


                var _orderQueryRepository = scope.ServiceProvider
                        .GetRequiredService<IOrderQueryRepository>();

                var model = this._orderQueryRepository.GetById(orderId);

                model.Completed();

                _orderCommandRepository.Update(model);


            }
               
        }

        public Task Execute(BehaviorContext<OrderStateInstance, SuccessfullyPaidEvent> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next)
        {
            CompleteOrder(context.Data.OrderId);

            return Task.CompletedTask;
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReserved, TException> context, Behavior<OrderStateInstance, StockReserved> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, SuccessfullyPaidEvent, TException> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }
    }
}
