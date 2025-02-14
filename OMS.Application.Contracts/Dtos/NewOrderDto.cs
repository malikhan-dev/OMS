using OMS.Domain.Orders.ValueObjects;

namespace OMS.Application.Contracts.Dtos
{
    public class NewOrderDto
    {
        public List<OrderItemValueObject> Items { get; set;} = new List<OrderItemValueObject>();
        public string Description { get; set; } = "";
    }
}
