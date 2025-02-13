using Microsoft.EntityFrameworkCore;
using OMS.Application.Services.EventPublisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
