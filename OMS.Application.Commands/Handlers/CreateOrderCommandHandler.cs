﻿using MediatR;
using OMS.Application.Commands.Commands;
using OMS.Application.Contracts.Services;

namespace OMS.Application.Commands.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderService _orderService;
        public CreateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orderService.CreateOrder(request.NewOrderDto));
        }
    }
}
