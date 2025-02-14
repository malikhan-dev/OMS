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
        private readonly IOrderCommandRepository _orderCommandRepository;
        private readonly IOrderQueryRepository _orderQueryRepository;
        public OrderPaidActivity(IOrderCommandRepository orderCommandRepository, IOrderQueryRepository orderQueryRepository)
        {
            _orderCommandRepository = orderCommandRepository;
            _orderQueryRepository = orderQueryRepository;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, SuccessfullyPaidEvent> context, Behavior<OrderStateInstance, SuccessfullyPaidEvent> next)
        {
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
