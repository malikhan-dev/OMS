using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMS.Infrastructure.Persistance.EF.Context;
using Microsoft.Extensions.Configuration;

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
