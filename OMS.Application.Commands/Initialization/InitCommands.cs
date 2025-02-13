﻿using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Commands.Commands;

namespace OMS.Application.Commands.Initialization
{
    public static class InitCommands
    {
        public static void Init(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>();
            });
        }
    }
}
