using Automatonymous;
using GreenPipes;
using OMS.Application.Services.Events;
using OMS.Application.Services.StateMachine.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Application.Services.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        private Event<CreateOrderMessage> CreateOrderMessage { get; set; }
        private Event<StockReserved> StockReservedEvent { get; set; }
        private Event<StockReservationFailed> StockReservationFailedEvent { get; set; }
        private Event<SuccessfullyPaidEvent> SuccessfullyPaidEvent { get; set; }
        private Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }
        private State OrderCreated { get; set; }
        private State OrderFailed { get; set; }
        private State StockReserved { get; set; }
        private State OrderPaid { get; set; }
        private State OrderCompleted { get; set; }
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CreateOrderMessage, y => y.CorrelateBy<Guid>(x => x.OrderId, z => z.Message.CorrelationId).SelectId(context => context.CorrelationId.Value));

            Initially(
              When(CreateOrderMessage)
             .Then(context =>
             {
                 context.Instance.OrderId = context.Data.CorrelationId;
                 context.Instance.CreatedDate = DateTime.UtcNow;
                 context.Instance.TotalPrice = context.Data.TotalPrice;
                 context.Instance.CorrelationString = context.Instance.CorrelationId.ToString();
             })
             .TransitionTo(OrderCreated)
             .Activity(c => c.OfType<OrderCreatedActivity>()));

            During(OrderCreated,
                When(StockReservedEvent)
                .Activity(c => c.OfType<OrderReservedActivity>())
                .TransitionTo(StockReserved));


            During(OrderCreated,
              When(SuccessfullyPaidEvent)
                .Activity(c => c.OfType<OrderPaidActivity>())
                .TransitionTo(OrderPaid));


            During(StockReserved,
                When(SuccessfullyPaidEvent)
                    .Activity(c => c.OfType<OrderCompletedActivity>())
                    .TransitionTo(OrderCompleted));

            During(OrderPaid,
                When(StockReservedEvent)
                .Activity(c => c.OfType<OrderCompletedActivity>())
                .TransitionTo(OrderCompleted)

            );

            DuringAny(When(StockReservationFailedEvent)
                .Activity(c => c.OfType<OrderFailedByReservationActivity>())
                .TransitionTo(OrderFailed));


            DuringAny(When(PaymentFailedEvent)
                .Activity(c => c.OfType<OrderFailedByPaymentActivity>())
                .TransitionTo(OrderFailed));

        }



    }
}
