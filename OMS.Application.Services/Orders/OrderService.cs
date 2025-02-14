using Grpc.Net.Client;
using InventoryService.Proto;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Configuration;
using OMS.Application.Services.StateMachine;
using OMS.Domain.Orders;
using OMS.Domain.Orders.Repositories;
using PaymentService.Proto;
using OrderItem = OMS.Application.Services.Events.OrderItem;
using OMS.Application.Services.Events;
using OMS.Application.Contracts.Dtos;
using OMS.Application.Contracts.Services;
using OMS.Application.Services.EventPublisher;
using Newtonsoft.Json;

namespace OMS.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly string _PayServerAddress;

        private readonly string _InventoryServerAddress;

        private readonly IOrderCommandRepository _orderCommandRepository;


        private readonly AppEventPublisher appEventPublisher;

        public OrderService(IConfiguration configuration, IOrderCommandRepository orderCommandRepository, AppEventPublisher appEventPublisher)
        {
            _PayServerAddress = configuration.GetSection("PayServer")?.Value ?? "";

            _InventoryServerAddress = configuration.GetSection("InventoryServer")?.Value ?? "";

            _orderCommandRepository = orderCommandRepository;

            this.appEventPublisher = appEventPublisher;
        }


      
        public bool CreateOrder(NewOrderDto dto)
        {
            var order = Order.Factory(dto.Items, dto.Description);
            
            _orderCommandRepository.Add(order);
         
            var message = new CreateOrderMessage()
            {
                CorrelationId = order.Id,

                OrderItemList = dto.Items.Select(x => new OrderItem() { Count = x.Count, ProductId = x.ProductId }).ToList(),

                TotalPrice = order.TotalPrice,

                Description = order.Description,
            };

            appEventPublisher.AddEvent(new AppOutBox() { Content = JsonConvert.SerializeObject(message), Type = message.GetType()?.AssemblyQualifiedName ?? "", });


            Pay(order.Id);

            Inventory(order.Id);

            return true;
        }

        private void Inventory(Guid orderId)
        {

            using var channel = GrpcChannel.ForAddress(_InventoryServerAddress);

            var client = new InventoryService.Proto.Inventory.InventoryClient(channel);

            client.CheckInventory(new InventoryCheckRequest { OrderId = orderId.ToString() });

        }

        private void Pay(Guid orderId)
        {
            using var channel = GrpcChannel.ForAddress(_PayServerAddress);

            var client = new PaymentService.Proto.Pay.PayClient(channel);

            client.Pay(new PayRequest { OrderId = orderId.ToString() });

        }
    }
}
