namespace OMS.Domain.Orders
{
    public enum OrderState
    {
        Processed = 0,
        Paid = 1,
        Reserved = 2,
        Completed = 3,
        Failed = 4
    }
}
