using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderCreatedActivity : Activity<OrderStateInstance, CreateOrderMessage>
    {
        public void Accept(StateMachineVisitor visitor)
        {
        }

        public Task Execute(BehaviorContext<OrderStateInstance, CreateOrderMessage> context, Behavior<OrderStateInstance, CreateOrderMessage> next)
        {
            //Event Source Order Creation

            return Task.CompletedTask;
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, CreateOrderMessage, TException> context, Behavior<OrderStateInstance, CreateOrderMessage> next) where TException : Exception
        {
            throw new NotImplementedException();
        }

        public void Probe(ProbeContext context)
        {
        }
    }

}
