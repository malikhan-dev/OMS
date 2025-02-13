using Dapper;
using Hangfire;
using Hangfire.Common;
using Magnum.Monads.Parser;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.IO;
using OMS.Application.Services.EventPublisher;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

                RecurringJob.AddOrUpdate(() => scopedProcessingService.Begin(), Cron.Minutely);


            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
