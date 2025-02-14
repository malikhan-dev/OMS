using Microsoft.EntityFrameworkCore;
using OMS.Application.Services.EventPublisher;

namespace OMS.Infrastructure.Persistance.EF.Context
{
    public class OutboxDbContext : DbContext
    {
        public DbSet<AppOutBox> OutBoxes { get; set; }

        public OutboxDbContext(DbContextOptions<OutboxDbContext> opt) : base(opt)
        {

        }

    }
}
