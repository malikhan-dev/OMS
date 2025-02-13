using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Contracts.Services;
using OMS.Application.Services.Orders;
using OMS.Application.Services.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Application.Services.Init
{
    public static class InitializeApp
    {
        public static void InitializeApplicationService(this IServiceCollection Services)
        {
           

            Services.AddScoped<IOrderService, OrderService>();
        }

        public static void InitMassTransit(this IServiceCollection Services)
        {
            Services.AddMassTransit(cfg =>
            {
                cfg.SetSnakeCaseEndpointNameFormatter();
                cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().MongoDbRepository(r =>
                {
                    r.Connection = "mongodb://127.0.0.1:27017";
                    r.DatabaseName = "OrderSaga";
                });


                cfg.UsingRabbitMq((cnt, prv) =>
                {

                    prv.ConfigureEndpoints(cnt);
                    prv.Host("localhost", "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });



                });
            });
            Services.AddMassTransitHostedService();
        }
    }
}
