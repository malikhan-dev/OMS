using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Orders;

namespace OMS.Infrastructure.Persistance.EF.EntityConfigurations
{
    internal class OrderItemEntityConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
