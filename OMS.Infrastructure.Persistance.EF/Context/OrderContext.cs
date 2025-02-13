using Microsoft.EntityFrameworkCore;
using OMS.Domain.Orders;

namespace OMS.Infrastructure.Persistance.EF.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> opt) : base(opt)
        {

        }
        public DbSet<Order> Orders { get; set; }
    }
}
