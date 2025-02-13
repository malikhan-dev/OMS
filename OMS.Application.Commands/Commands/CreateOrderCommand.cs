using MediatR;
using OMS.Application.Contracts.Dtos;

namespace OMS.Application.Commands.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public readonly NewOrderDto NewOrderDto;
        public CreateOrderCommand(NewOrderDto dto)
        {
            NewOrderDto = dto;
        }
    }
}
