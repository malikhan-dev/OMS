using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Orders;

namespace OMS.Infrastructure.Persistance.EF.EntityConfigurations
{
    public class OrderEntityConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(x => x.Items).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
        }
    }
}
