using System.Text;

using EventStore.ClientAPI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace OMS.Application.Services.Events.Services
{
    public class EventStorage
    {
        private readonly IEventStoreConnection _connection;

        private readonly string EventStoreDbConnection;
        public EventStorage(IConfiguration configuration)
        {
            EventStoreDbConnection = configuration.GetSection("ESDB").Value;

            _connection = EventStoreConnection.Create(EventStoreDbConnection, ConnectionSettings.Create().KeepReconnecting(), "orders-manager");

            _connection.ConnectAsync();
        }
        public async Task AppendEvents(object data, string streamName)
        {
            var eventData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var eventPayload = new EventData(Guid.NewGuid(), data.GetType().AssemblyQualifiedName, true, eventData, null);
            await _connection.AppendToStreamAsync(streamName, events: eventPayload, expectedVersion: ExpectedVersion.Any);
        }
    }
}
