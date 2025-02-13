using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace OMS.Infrastructure.Messaging.Masstransit
{
    public static class Initialization
    {
        public static void InitMassTransit(this IServiceCollection Services)
        {
            Services.AddMassTransit(cfg =>
            {
                cfg.SetSnakeCaseEndpointNameFormatter();
                cfg.AddConsumer<testconsumer>();
                //cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().InMemoryRepository();
                cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().MongoDbRepository(r =>
                {
                    r.Connection = "mongodb://127.0.0.1:27017";
                    r.DatabaseName = "OrderSaga";
                });
                //cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
                //{


                //    opt.AddDbContext<DbContext, StateMachineContext>((provider, builder) =>
                //    {
                //        builder.UseSqlite("Data Source=mydb.db",
                //            m => { m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name); });
                //    });

                //    //opt.ConcurrencyMode = ConcurrencyMode.Optimistic;
                //});





                //cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
                //{


                //    opt.AddDbContext<DbContext, StateMachineContext>((provider, builder) =>
                //    {
                //        builder.UseSqlite("Data Source=mydb.db",
                //            m => { m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name); });
                //    });

                //    //opt.ConcurrencyMode = ConcurrencyMode.Optimistic;
                //});

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
