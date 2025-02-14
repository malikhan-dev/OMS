using OMS.Domain.Orders;
using OMS.Domain.Orders.Repositories;
using OMS.Infrastructure.Persistance.EF.Context;

namespace OMS.Infrastructure.Persistance.EF.Repositories
{
    public class OrderQueryRepository : IOrderQueryRepository
    {
        private readonly OrderContext _context;
        public OrderQueryRepository(OrderContext context)
        {
            _context = context;
        }
        public Order GetById(Guid id)
        {
            return _context.Orders.Find(id);
        }
    }
}
