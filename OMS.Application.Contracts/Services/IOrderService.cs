using OMS.Application.Contracts.Dtos;

namespace OMS.Application.Contracts.Services
{
    public interface IOrderService
    {
        bool CreateOrder(NewOrderDto dto);
    }
}
