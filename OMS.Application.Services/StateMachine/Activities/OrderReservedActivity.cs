using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderReservedActivity : Activity<OrderStateInstance, StockReserved>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public Task Execute(BehaviorContext<OrderStateInstance, StockReserved> context, Behavior<OrderStateInstance, StockReserved> next)
        {
            return Task.CompletedTask;
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
