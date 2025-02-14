using MassTransit;

namespace OMS.Application.Services.Events
{
    public class StockReservationFailed : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
    }
}
