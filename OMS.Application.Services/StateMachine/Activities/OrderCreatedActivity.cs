using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderCreatedActivity : Activity<OrderStateInstance, CreateOrderMessage>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, CreateOrderMessage> context, Behavior<OrderStateInstance, CreateOrderMessage> next)
        {
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, CreateOrderMessage, TException> context, Behavior<OrderStateInstance, CreateOrderMessage> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("publish-order-closed");
        }
     
    }

}
