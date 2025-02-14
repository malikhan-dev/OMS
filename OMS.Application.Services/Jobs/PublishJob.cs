using Dapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Services.EventPublisher;
using System.Data;

namespace OMS.Application.Services.Jobs
{
    public class PublishJob
    {
        public IDbConnection _connection { get; }
        private readonly IPublishEndpoint eventPublisher;

        public PublishJob([FromKeyedServices("OutBoxConnection")] IDbConnection connection, IPublishEndpoint publisher)
        {
            _connection = connection;
            eventPublisher = publisher;
        }


        public async Task Begin()
        {
            using (var connection = _connection)
            {
                try
                {
                    connection.Open();

                    var result = connection.Query<AppOutBox>("SELECT TOP (50) [Id] ,[Content],[Type] ,[Published] ,[RetryCount] FROM [dbo].[OutBoxes] where Published = 0 and RetryCount < 20 order by id asc");

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            try
                            {
                                var res = Newtonsoft.Json.JsonConvert.DeserializeObject(item.Content, Type.GetType(item.Type));

                                await eventPublisher.Publish(res);

                                var command = connection.Execute("update dbo.OutBoxes set RetryCount = RetryCount + 1, Published = 1 where id = @id", new { @id = item.Id });
                            }
                            catch
                            {
                                var command = connection.Execute("update dbo.OutBoxes set RetryCount = RetryCount +1 where id = @id", new { @id = item.Id });
                            }

                            Task.Delay(1000).Wait();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }

}
