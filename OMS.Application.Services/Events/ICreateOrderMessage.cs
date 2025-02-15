﻿namespace OMS.Application.Services.Events
{
    public interface ICreateOrderMessage
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> OrderItemList { get; set; }
    }
}
