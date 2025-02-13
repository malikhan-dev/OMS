using Grpc.Core;
using MassTransit;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Events;
using PaymentService.Proto;

namespace PaymentService.Services
{
    public class PaymentHandler : PaymentService.Proto.Pay.PayBase
    {
        private readonly AppEventPublisher _eventPublisher;

        public PaymentHandler(AppEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public override Task<PayResponse> Pay(PayRequest request, ServerCallContext context)
        {

            var paid = Random.Shared.NextDouble() > 0.5;

            if (paid)
            {
                _eventPublisher.AddEvent(new AppOutBox()
                {
                    Content = Newtonsoft.Json.JsonConvert.SerializeObject(new SuccessfullyPaidEvent() { CorrelationId = Guid.Parse(request.OrderId), OrderId = Guid.Parse(request.OrderId) }),
                    Type = typeof(SuccessfullyPaidEvent).AssemblyQualifiedName,
                });
            }
            else
            {
                _eventPublisher.AddEvent(new AppOutBox()
                {
                    Content = Newtonsoft.Json.JsonConvert.SerializeObject(new PaymentFailedEvent() { CorrelationId = Guid.Parse(request.OrderId), OrderId = Guid.Parse(request.OrderId) }),
                    Type = typeof(PaymentFailedEvent).AssemblyQualifiedName,
                });
              
            }
            return Task.FromResult(new PayResponse());
        }

    }
}
