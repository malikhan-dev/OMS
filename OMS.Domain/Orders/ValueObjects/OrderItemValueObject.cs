namespace OMS.Domain.Orders.ValueObjects
{
    public class OrderItemValueObject
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }

        public OrderItemValueObject(int productId,int count, double unitprice)
        {
            this.ProductId = productId;
            this.Count = count;
            this.UnitPrice = unitprice;
        }
    }
}
