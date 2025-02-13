﻿using Automatonymous;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.Events;
using OMS.Application.Services.Jobs;
using OMS.Domain.Orders.Repositories;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderReservedActivity : Activity<OrderStateInstance, StockReserved>
    {

        private readonly IServiceProvider serviceProvider;
        public OrderReservedActivity(IServiceProvider Services)
        {
            this.serviceProvider = Services;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _orderCommandRepository =
                    scope.ServiceProvider
                        .GetRequiredService<IOrderCommandRepository>();


                var _orderQueryRepository = scope.ServiceProvider
                        .GetRequiredService<IOrderQueryRepository>();

                var model = _orderQueryRepository.GetById(context.Data.CorrelationId);

                model.Reserved();

                _orderCommandRepository.Update(model);

                await next.Execute(context).ConfigureAwait(false);

            }



        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, StockReserved, TException> context, Behavior<OrderStateInstance, StockReserved> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }
    }

}
