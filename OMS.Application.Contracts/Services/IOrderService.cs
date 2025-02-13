using OMS.Application.Contracts.Dtos;

namespace OMS.Application.Contracts.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(NewOrderDto dto);
    }
}
