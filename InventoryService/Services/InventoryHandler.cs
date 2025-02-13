using Grpc.Core;
using InventoryService.Proto;

namespace InventoryService.Services
{
    public class InventoryHandler : InventoryService.Proto.Inventory.InventoryBase
    {
        public override Task<InventoryCheckResponse> CheckInventory(InventoryCheckRequest request, ServerCallContext context)
        {
            return Task.FromResult(new InventoryCheckResponse()
            {
                Suceeded = true,
                Message = "Ok"
            });
        }
    }
}
