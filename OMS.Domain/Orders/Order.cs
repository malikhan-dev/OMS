using OMS.Domain.Contracts;
using OMS.Domain.Orders.ValueObjects;

namespace OMS.Domain.Orders
{
    public class Order : AggregateRoot<int>
    {
        public string Description { get; private set; } = string.Empty;
        public double TotalPrice { get; private set; }
        public OrderState State { get; private set; }
        public HashSet<OrderItem> Items { get; private set; } = new HashSet<OrderItem>();
        public static Order Factory(List<OrderItemValueObject> OrderItems, string Desc)
        {
            var order = new Order();
            
            order.SetDescription(Desc);
            
            order.InitOrder();

            foreach(var item in OrderItems)
            {
                order.AddItem(new OrderItem()
                {
                    Count = item.Count,
                    
                    ProductId = item.ProductId,
                   
                    UnitPrice = item.UnitPrice

                });
            }
            order.CalculateOrderPrice();

            return order;
        }

        public void CalculateOrderPrice()=> this.TotalPrice = this.Items.Sum(x => x.UnitPrice * x.Count);

        public void AddItem(OrderItem item) => this.Items.Add(item);

        public void SetDescription(string Description)
        {
            this.Description = Description;
        }
        public void InitOrder() => this.State = OrderState.Processed;

        public void Reserved() => this.State = OrderState.Reserved;


        public void Paid() => this.State = OrderState.Paid;

        public void Failed() => this.State = OrderState.Failed;
        public void Completed() => this.State = OrderState.Completed;

    }
}
