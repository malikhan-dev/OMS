using Dapper;
using Magnum.Monads.Parser;
using Magnum.Threading;
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
        private static bool Locked;
        private static object @locker = new object();

        public PublishJob([FromKeyedServices("OutBoxConnection")] IDbConnection connection, IPublishEndpoint publisher)
        {
            _connection = connection;
            eventPublisher = publisher;
        }


        public async Task Begin()
        {
            if (!Locked)
            {
                lock (@locker)
                {
                    Locked = !Locked;

                    using (var connection = _connection)
                    {
                        try
                        {
                            connection.Open();

                            var result = connection.Query<AppOutBox>("SELECT TOP (400) [Id] ,[Content],[Type] ,[Published] ,[RetryCount] FROM [dbo].[OutBoxes] where (Published = 0 and RetryCount < 20) order by date asc");

                            Dispatch(result, connection);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            connection.Close();

                        }
                    }

                    Locked = !Locked;
                }

            }
        }

        private void Dispatch(IEnumerable<AppOutBox> items, IDbConnection connection)
        {
            foreach (var item in items)
            {
                try
                {
                    var res = Newtonsoft.Json.JsonConvert.DeserializeObject(item.Content, Type.GetType(item.Type));

                    eventPublisher.Publish(res).GetAwaiter().GetResult();

                    var command = connection.Execute("update dbo.OutBoxes set RetryCount = RetryCount + 1, Published = 1 where id = @id", new { @id = item.Id });
                }
                catch
                {
                    var command = connection.Execute("update dbo.OutBoxes set RetryCount = RetryCount +1 where id = @id", new { @id = item.Id });
                }

                Task.Delay(100).Wait();
            }
        }

    }

}
