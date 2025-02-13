namespace OMS.Domain.Orders.Repositories
{
    public interface IOrderCommandRepository
    {
        Guid Add(Order order);
    }
}
