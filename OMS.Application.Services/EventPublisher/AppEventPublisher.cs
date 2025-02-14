using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace OMS.Application.Services.EventPublisher
{
    public class AppEventPublisher
    {
        private readonly IDbConnection _connection;
        public AppEventPublisher([FromKeyedServices("OutBoxConnection")] IDbConnection connection)
        {
            _connection = connection;
        }
        public void AddEvent(AppOutBox @event)
        {
            using (_connection)
            {
                try
                {
                    _connection.Open();
                    
                    string insertQuery = @"INSERT INTO [dbo].[OutBoxes]( [Content], [Type], [Published], [RetryCount] ) Values (@Content,@Type,@Published,@RetryCount) ";

                    var result = _connection.Execute(insertQuery, new
                    {
                        @Content = @event.Content,
                        @Type = @event.Type,
                        @Published = false,
                        @RetryCount = 0,
                    });

                    _connection.Close();
                }
                finally
                {
                    _connection?.Close();
                }
            }
        }
    }
}
