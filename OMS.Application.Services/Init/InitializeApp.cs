using MassTransit;
using Microsoft.Data.SqlClient;
using OMS.Application.Contracts.Services;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Jobs;
using OMS.Application.Services.Orders;
using OMS.Application.Services.StateMachine;
using System.Data;
using Hangfire;
using Hangfire.MemoryStorage;
using OMS.Application.Services.StateMachine.Activities;
using Microsoft.Extensions.DependencyInjection;
namespace OMS.Application.Services.Init
{
    public static class InitializeApp
    {
        public static void InitializeApplicationService(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services, string OutboxConnectionString)
        {
            Services.AddKeyedTransient<IDbConnection, SqlConnection>("OutBoxConnection", (ServiceProvider, cnt) => new SqlConnection(OutboxConnectionString));

            Services.AddHangfire(c => c.UseMemoryStorage());

            Services.AddHangfireServer();

            Services.AddScoped<OrderPaidActivity>();

            Services.AddScoped<OrderReservedActivity>();

            Services.AddScoped<OrderCompletedActivity>();
           
            Services.AddHostedService<EventPublisherJob>();

            Services.AddScoped<PublishJob>();

            Services.AddScoped<AppEventPublisher>();

            Services.AddScoped<IOrderService, OrderService>();
        }

        public static void InitMassTransit(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services)
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
