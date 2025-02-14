using Automatonymous;
using EventStore.Client;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Application.Services.Events.Services;
using System.Text.Json;

namespace OMS.Application.Services.StateMachine.Activities
{
    public class OrderCreatedActivity : Activity<OrderStateInstance, CreateOrderMessage>
    {
        private readonly EventStorage eventStoreService;

        public OrderCreatedActivity(EventStorage eventStoreService)
        {
            this.eventStoreService = eventStoreService;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderStateInstance, CreateOrderMessage> context, Behavior<OrderStateInstance, CreateOrderMessage> next)
        {
            string @eventLog = $"Order: {context.Data.CorrelationId} Created";

            await eventStoreService.AppendEvents(eventLog,nameof(OrderCreatedActivity));
            
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
