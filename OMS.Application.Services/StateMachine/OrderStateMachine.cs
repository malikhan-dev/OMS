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

            Event(() => CreateOrderMessage, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Initially(
              When(CreateOrderMessage)
             .Then(context =>
             {
                 context.Instance.OrderId = context.Data.OrderId;
                 context.Instance.CreatedDate = DateTime.UtcNow;
                 context.Instance.TotalPrice = context.Data.TotalPrice;
                 context.Instance.CorrelationString = context.Instance.CorrelationId.ToString();
             })
             .TransitionTo(OrderCreated)
             .Activity(c => c.OfType<OrderCreatedActivity>()));

            During(OrderCreated,
                When(StockReservedEvent)
                    .TransitionTo(StockReserved)
                        .Activity(c => c.OfType<OrderReservedActivity>())
                    );


            During(OrderCreated,
              When(SuccessfullyPaidEvent)
                  .TransitionTo(OrderPaid)
                  );


            During(StockReserved,
                When(SuccessfullyPaidEvent)
                   .TransitionTo(OrderCompleted)
                   );

            During(OrderPaid,
                When(StockReservedEvent)
                .TransitionTo(OrderCompleted)
            );

            DuringAny(When(StockReservationFailedEvent)
                        .TransitionTo(OrderFailed));

            DuringAny(When(PaymentFailedEvent)
                      .TransitionTo(OrderFailed));

        }



    }
}
