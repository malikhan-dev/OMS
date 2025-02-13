using Grpc.Core;
using InventoryService.Proto;
using MassTransit;
using MongoDB.Bson.IO;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Events;

namespace InventoryService.Services
{
    public class InventoryHandler : InventoryService.Proto.Inventory.InventoryBase
    {
        private readonly AppEventPublisher _eventPublisher;

        public InventoryHandler(AppEventPublisher publishEndpoint)
        {
            this._eventPublisher = publishEndpoint;
        }


        public override Task<InventoryCheckResponse> CheckInventory(InventoryCheckRequest request, ServerCallContext context)
        {
        

            var reserved = Random.Shared.NextDouble() > 0.5;

            if (reserved)
            {
                _eventPublisher.AddEvent(new AppOutBox()
                {
                    Content = Newtonsoft.Json.JsonConvert.SerializeObject(new StockReserved() { CorrelationId = Guid.Parse(request.OrderId) }),
                    Type = typeof(StockReserved).AssemblyQualifiedName,

                });

            }
            else
            {
                _eventPublisher.AddEvent(new AppOutBox()
                {
                    Content = Newtonsoft.Json.JsonConvert.SerializeObject(new StockReservationFailed() { CorrelationId = Guid.Parse(request.OrderId) }),
                    Type = typeof(StockReservationFailed).AssemblyQualifiedName,

                });
            }
            return Task.FromResult(new InventoryCheckResponse());
        }
    }
}
