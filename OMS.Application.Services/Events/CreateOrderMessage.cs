using MassTransit;

namespace OMS.Application.Services.Events
{
    public class CreateOrderMessage :   CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> OrderItemList { get; set; }
        public string Description { get; set; }

    }

    
}
