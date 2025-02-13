using Grpc.Core;
using PaymentService.Proto;

namespace PaymentService.Services
{
    public class PaymentHandler : PaymentService.Proto.Pay.PayBase
    {
        public PaymentHandler()
        {
            
        }

        public override Task<PayResponse> Pay(PayRequest request, ServerCallContext context)
        {
            return Task.FromResult( new PayResponse()
            {
                Suceeded = true,
                Message = "paid"
            });
        }

    }
}
