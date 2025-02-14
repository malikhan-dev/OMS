using Microsoft.Data.SqlClient;
using OMS.Application.Contracts.Services;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Jobs;
using OMS.Application.Services.Orders;
using System.Data;
using Hangfire;
using Hangfire.MemoryStorage;
using OMS.Application.Services.StateMachine.Activities;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.Events.Services;
namespace OMS.Application.Services.Init
{
    public static class InitializeApp
    {
        public static void InitializeApplicationService(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services, string OutboxConnectionString)
        {
            Services.AddKeyedTransient<IDbConnection, SqlConnection>("OutBoxConnection", (ServiceProvider, cnt) => new SqlConnection(OutboxConnectionString));

            Services.AddScoped<AppEventPublisher>();

            Services.AddScoped<OrderPaidActivity>();

            Services.AddScoped<OrderReservedActivity>();

            Services.AddScoped<OrderCompletedActivity>();

            Services.AddScoped<IOrderService, OrderService>();

            Services.AddScoped<EventStorage>();
            
        }

        public static void InitializeJobs(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services)
        {
            Services.AddHostedService<EventPublisherJob>();

            Services.AddHangfire(c => c.UseMemoryStorage());

            Services.AddHangfireServer();

            Services.AddScoped<PublishJob>();

        }
    }
}
