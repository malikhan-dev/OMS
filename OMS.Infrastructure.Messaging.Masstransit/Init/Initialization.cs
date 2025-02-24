using Automatonymous;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.StateMachine;

namespace OMS.Infrastructure.Messaging.Masstransit.Init
{
    public static class Initialization
    {

        public static void InitMasstransit(this IServiceCollection serviceCollection, string mongoDbConnection = "mongodb://127.0.0.1:27017")
        {
            serviceCollection.AddMassTransit(cfg =>
            {
                cfg.SetSnakeCaseEndpointNameFormatter();
                

                cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>(cfg =>
                {
                    cfg.UseMessageRetry(x =>
                    {
                        x.Interval(200, TimeSpan.FromMicroseconds(100));
                    });

                }).MongoDbRepository(r =>
                {
                    r.Connection = mongoDbConnection;

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
            serviceCollection.AddMassTransitHostedService();
        }
        public static void InitMasstransit<TStateMachine, TInstance>(this IServiceCollection serviceCollection, string mongoDbConnection = "mongodb://127.0.0.1:27017") where TStateMachine : class, SagaStateMachine<TInstance> where TInstance : class, SagaStateMachineInstance, ISagaVersion
        {
            serviceCollection.AddMassTransit(cfg =>
            {
                cfg.SetSnakeCaseEndpointNameFormatter();

                cfg.AddSagaStateMachine<TStateMachine, TInstance>(cfg =>
                {

                }).MongoDbRepository(r =>
                {
                    r.Connection = mongoDbConnection;

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
            serviceCollection.AddMassTransitHostedService();
        }

        public static void InitMasstransit(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMassTransit(cfg =>
            {
                
                cfg.SetSnakeCaseEndpointNameFormatter();

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
            serviceCollection.AddMassTransitHostedService();
        }
    }
}
