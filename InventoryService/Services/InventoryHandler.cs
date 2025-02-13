using Grpc.Core;
using InventoryService.Proto;
using MassTransit;
using OMS.Application.Services.Events;

namespace InventoryService.Services
{
    public class InventoryHandler : InventoryService.Proto.Inventory.InventoryBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public InventoryHandler(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }


        public override Task<InventoryCheckResponse> CheckInventory(InventoryCheckRequest request, ServerCallContext context)
        {
        

            var reserved = Random.Shared.NextDouble() > 0.5;

            if (reserved)
            {
                this.publishEndpoint.Publish(new StockReserved() { CorrelationId = Guid.Parse(request.OrderId)});
            }
            else
            {
                this.publishEndpoint.Publish(new StockReservationFailed() { CorrelationId = Guid.Parse(request.OrderId)});
            }
            return Task.FromResult(new InventoryCheckResponse());
        }
    }
}
