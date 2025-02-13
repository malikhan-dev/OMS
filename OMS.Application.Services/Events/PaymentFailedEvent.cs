using MassTransit;

namespace OMS.Application.Services.Events
{
    public class PaymentFailedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
    }
}
