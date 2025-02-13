using MassTransit;

namespace OMS.Application.Services.Events
{
    public class StockReserved : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}
