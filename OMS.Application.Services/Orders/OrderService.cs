using Grpc.Net.Client;
using InventoryService.Proto;
using Microsoft.Extensions.Configuration;
using OMS.Application.Contracts.Dtos;
using OMS.Application.Contracts.Services;
using OMS.Domain.Orders;
using OMS.Domain.Orders.Repositories;
using PaymentService.Proto;

namespace OMS.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration configuration;
        private readonly string _PayServerAddress;
        private readonly string _InventoryServerAddress;
        private readonly IOrderCommandRepository _orderCommandRepository;
        public OrderService(IConfiguration configuration, IOrderCommandRepository orderCommandRepository)
        {
            _PayServerAddress = configuration.GetSection("PayServer")?.Value ?? "";

            _InventoryServerAddress = configuration.GetSection("InventoryServer")?.Value ?? "";

            _orderCommandRepository = orderCommandRepository;
        }


        public async Task<bool> CreateOrder(NewOrderDto dto)
        {
            var order = Order.Factory(dto.Items, dto.Description);

            _orderCommandRepository.Add(order);
            
            CheckInventory(order.Id);
            
            CheckPayment(order.Id);
            
            return true;
        }

        private void CheckInventory(int orderId)
        {

            using var channel = GrpcChannel.ForAddress(_InventoryServerAddress);

            var client = new InventoryService.Proto.Inventory.InventoryClient(channel);

            client.CheckInventory(new InventoryCheckRequest { OrderId = orderId });

        }

        private void CheckPayment(double Amount)
        {
            using var channel = GrpcChannel.ForAddress(_PayServerAddress);

            var client = new PaymentService.Proto.Pay.PayClient(channel);

            client.Pay(new PayRequest { Price = 200 });
          
        }
    }
}
