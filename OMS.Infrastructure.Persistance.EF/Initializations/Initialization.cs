using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OMS.Domain.Orders.Repositories;
using OMS.Infrastructure.Persistance.EF.Context;
using OMS.Infrastructure.Persistance.EF.Repositories;

namespace OMS.Infrastructure.Persistance.EF.Initializations
{
    public static class Initialization
    {
        public static void InjectSqlServerEfCoreDependencies(this IServiceCollection services, string connectionstring)
        {
            services.AddDbContext<OrderContext>(cfg =>
            {
                cfg.UseSqlServer(connectionstring);
            });
            services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
            services.AddScoped<IOrderCommandRepository, OrderRepository>();

        }

        public static void InjectOutboxDb(this IServiceCollection services, string connectionstring)
        {
            services.AddDbContext<OutboxDbContext>(cfg =>
            {
                cfg.UseSqlServer(connectionstring);
            });
           

        }
    }
}
