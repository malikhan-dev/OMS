namespace OMS.Domain.Orders.Repositories
{
    public interface IOrderCommandRepository
    {
        int Add(Order order);
    }
}
