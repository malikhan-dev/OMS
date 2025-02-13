using MassTransit;

namespace OMS.Application.Services.Events
{
    public class SuccessfullyPaidEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

    }
}
