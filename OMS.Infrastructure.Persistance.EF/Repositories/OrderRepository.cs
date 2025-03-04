﻿using OMS.Domain.Orders;
using OMS.Domain.Orders.Repositories;
using OMS.Infrastructure.Persistance.EF.Context;

namespace OMS.Infrastructure.Persistance.EF.Repositories
{
    public class OrderRepository : IOrderCommandRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }
        public Guid Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
            
        }
    }
}
