using Microsoft.EntityFrameworkCore;
using OMS.Domain.Orders;
using OMS.Infrastructure.Persistance.EF.EntityConfigurations;

namespace OMS.Infrastructure.Persistance.EF.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> opt) : base(opt)
        {

        }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityConfig());
            modelBuilder.ApplyConfiguration(new OrderItemEntityConfig());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Order> Orders { get; set; }
    }
}
