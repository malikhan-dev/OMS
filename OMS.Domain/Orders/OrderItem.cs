using OMS.Domain.Contracts;

namespace OMS.Domain.Orders
{
    public class OrderItem : AggregatedBy<Order,int>
    {
        public int ProductId { get;  set; }
        public int Count { get;  set; }
        public double UnitPrice { get;  set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
    }
}
