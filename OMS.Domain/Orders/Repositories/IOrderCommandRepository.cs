namespace OMS.Domain.Orders.Repositories
{
    public interface IOrderCommandRepository
    {
        void Add(Order order);
    }
}
