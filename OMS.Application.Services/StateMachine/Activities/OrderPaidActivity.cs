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
    internal class OrderPaidActivity : Activity<OrderStateInstance, SuccessfullyPaidEvent>
    {        
        private readonly IServiceProvider _ServiceProvider;
        public OrderPaidActivity(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, SuccessfullyPaidEvent> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next)
        {
            using (var scope = _ServiceProvider.CreateScope())
            {
                var _orderCommandRepository =
                    scope.ServiceProvider
                        .GetRequiredService<IOrderCommandRepository>();


                var _orderQueryRepository = scope.ServiceProvider
                        .GetRequiredService<IOrderQueryRepository>();

                var model = _orderQueryRepository.GetById(context.Data.CorrelationId);

                model.Paid();

                _orderCommandRepository.Update(model);

                await next.Execute(context).ConfigureAwait(false);


            }

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
