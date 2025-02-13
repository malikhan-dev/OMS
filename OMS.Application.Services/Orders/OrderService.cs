using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using OMS.Application.Contracts.Dtos;
using OMS.Application.Contracts.Services;
using PaymentService.Proto;

namespace OMS.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration configuration;
        private readonly string _PayServerAddress;

        public OrderService(IConfiguration configuration)
        {
            _PayServerAddress = configuration.GetSection("PayServer").Value;
        }


        public async Task<bool> CreateOrder(NewOrderDto dto)
        {
            using var channel = GrpcChannel.ForAddress(_PayServerAddress);

            var client = new PaymentService.Proto.Pay.PayClient(channel);

            var result =  client.Pay(new PayRequest { Price = 200});

            return result.Suceeded;
        }
    }
}
