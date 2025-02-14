using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OMS.Application.Services.Jobs
{
    internal class EventPublisherJob : IHostedService
    {
        public IServiceProvider Services { get; }

        public EventPublisherJob(IServiceProvider services)
        {
            Services = services;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<PublishJob>();

                RecurringJob.AddOrUpdate(() => scopedProcessingService.Begin(),Cron.Minutely());


            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
