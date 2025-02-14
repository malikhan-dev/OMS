namespace OMS.Application.Services.Events;

public class OrderItem
{
    public int ProductId { get; set; }
    public int Count { get; set; }
    public double UnitPrice { get; set; }
}