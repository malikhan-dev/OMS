using OMS.Domain.Orders.ValueObjects;

namespace OMS.Application.Contracts.Dtos
{
    public class NewOrderDto
    {
        public List<OrderItemValueObject> Items { get; set;}
        public string Description { get; set; }
    }
}
