using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using OMS.Infrastructure.Persistance.EF.Context;

namespace OMS.Infrastructure.Persistance.EF.Design
{
    internal class DesignTimeOutBoxContextFactory : IDesignTimeDbContextFactory<OutboxDbContext>
    {
        public OutboxDbContext CreateDbContext(string[] args)
        {
            string MigrationConnectionString = "Data Source=localhost,1433;Initial Catalog=OutBox;Integrated Security = true;TrustServerCertificate=True";


            var builder = new DbContextOptionsBuilder<OutboxDbContext>();
            

            builder.UseSqlServer(MigrationConnectionString);


            return new OutboxDbContext(builder.Options);
        }
    }
}
