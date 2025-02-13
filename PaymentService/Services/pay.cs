using Grpc.Core;
using MassTransit;
using OMS.Application.Services.Events;
using PaymentService.Proto;

namespace PaymentService.Services
{
    public class PaymentHandler : PaymentService.Proto.Pay.PayBase
    {
        private readonly IPublishEndpoint _endpoint;
        public PaymentHandler(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public override Task<PayResponse> Pay(PayRequest request, ServerCallContext context)
        {

            var paid = Random.Shared.NextDouble() > 0.5;

            if (paid)
            {
                this._endpoint.Publish(new SuccessfullyPaidEvent() {  CorrelationId = Guid.Parse(request.OrderId), OrderId = Guid.Parse( request.OrderId) });
            }
            else
            {
                this._endpoint.Publish(new PaymentFailedEvent() { CorrelationId = Guid.Parse(request.OrderId), OrderId = Guid.Parse(request.OrderId) });
            }
            return Task.FromResult(new PayResponse());
        }

    }
}
