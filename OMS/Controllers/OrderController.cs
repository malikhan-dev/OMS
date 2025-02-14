using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.Commands.Commands;
using OMS.Application.Contracts.Dtos;

namespace OMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;
        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost(Name = "NewOrder")]
        public async Task<IActionResult> NewOrder([FromBody] NewOrderDto dto)
        {
            await mediator.Send(new CreateOrderCommand(dto));
            return Ok();
        }
    }
}
