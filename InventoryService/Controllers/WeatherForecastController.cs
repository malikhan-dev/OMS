using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.Services.EventPublisher;
using OMS.Application.Services.Events;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {

        private readonly AppEventPublisher _eventPublisher;

        public InventoryController(AppEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        [HttpPut(Name = "Inventory-Confirm")]
        public OkResult Confirm([FromQuery] Guid guid)
        {
            var reserved = Random.Shared.NextDouble() > 0.5;

            if (reserved)
            {
                _eventPublisher.AddEvent(new AppOutBox()
                {
                    Content = Newtonsoft.Json.JsonConvert.SerializeObject(new StockReserved() { CorrelationId = guid }),
                    Type = typeof(StockReserved).AssemblyQualifiedName,

                });

            }
            else
            {
                _eventPublisher.AddEvent(new AppOutBox()
                {
                    Content = Newtonsoft.Json.JsonConvert.SerializeObject(new StockReservationFailed() { CorrelationId = guid }),
                    Type = typeof(StockReservationFailed).AssemblyQualifiedName,

                });
            }

            return Ok();
        }
    }
}
