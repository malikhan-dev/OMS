using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Contracts.Services;
using OMS.Application.Services.Orders;
using OMS.Domain.Orders.Repositories;
using OMS.Infrastructure.Persistance.EF.Context;
using OMS.Infrastructure.Persistance.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderCommandRepository, OrderRepository>();

        }
    }
}
