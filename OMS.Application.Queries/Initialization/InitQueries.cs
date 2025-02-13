﻿using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Queries.Queries;

namespace OMS.Application.Queries.Initialization
{
    public static class InitQueries
    {
        public static void Init(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<GetOrderQuery>();
            });
        }
    }
}
