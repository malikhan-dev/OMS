using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using OMS.Infrastructure.Persistance.EF.Context;

namespace OMS.Infrastructure.Persistance.EF.Design
{
    internal class DesignTimeOrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            string MigrationConnectionString = "Data Source=localhost,1433;Initial Catalog=OMS;User Id=sa;password=Your_password123;TrustServerCertificate=True";


            var builder = new DbContextOptionsBuilder<OrderContext>();
            

            builder.UseSqlServer(MigrationConnectionString);


            return new OrderContext(builder.Options);
        }
    }


}
